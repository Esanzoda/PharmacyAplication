using Grpc.Contract.Proto;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using ProductService.CQRS.Product.Mappers;
using ProductService.Exception;
using ProductService.Interfaces;
using ProductGrpc = Grpc.Contract.Proto.ProductService; 

namespace ProductService.Service.Grpc
{
    public class ProductGrpcService(
        IProductDbContext dbContext)
        : ProductGrpc.ProductServiceBase
    {
        public override async Task<ProductsGrpcResponseFromProductService>
            GetProducts(
                ProductGrpcIdsRequest request,
                ServerCallContext context)
        {
            var productIds = request.ProductIds
                .ToList();

            var products = await dbContext.Products
                .Where(x => productIds.Contains(x.Id))
                .ToListAsync(context.CancellationToken);

            return ProductMappers.ToProductsGrpcResponseFromProductService(products);
        }


        public override async Task<UpdateStockResponse> UpdateStock(
            UpdateStockRequest request,
            ServerCallContext context)
        {
            var requestedItems = request.OrderGrpcRequest
                .GroupBy(x => x.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    Quantity = g.Sum(x => x.Quantity)
                })
                .ToList();

            var productIds = requestedItems
                .Select(x => x.ProductId)
                .Distinct()
                .ToList();

            var products = await dbContext.Products
                .Where(x => productIds.Contains(x.Id))
                .ToDictionaryAsync(x => x.Id);

            var batches = await dbContext.ProductBatches
                .Where(x => productIds.Contains(x.ProductEntityId)
                            && x.Quantity > 0
                            && x.IsActive)
                .OrderBy(x => x.ExpiryDate)
                .ToListAsync(context.CancellationToken);

            var productBatches = batches
                .GroupBy(x => x.ProductEntityId)
                .ToDictionary(x => x.Key, x => x.ToList());


            foreach (var item in requestedItems)
            {
                var requestQuantity = item.Quantity;
                if (!products.TryGetValue(item.ProductId, out var product))
                {
                    throw new ResourceNotFoundException("Product not found");
                }


                if (!productBatches.TryGetValue(item.ProductId, out var productBatch))
                {
                    throw new ResourceNotFoundException("Product batch  not found");
                }

                var availableQuantity = productBatch.Sum(x => x.Quantity);
                if (item.Quantity > availableQuantity ||
                    item.Quantity > product.Stock)
                {
                    return new UpdateStockResponse
                    {
                        Success = false,
                        Message = $"Insufficient stock for {product.Name}."
                    };
                }

                foreach (var batch in productBatch)
                {
                    if (requestQuantity != 0)
                    {
                        var branchQuantity = batch.Quantity;

                        if (requestQuantity > branchQuantity)
                        {
                            batch.Quantity -= branchQuantity;
                            requestQuantity -= branchQuantity;
                            batch.IsActive = false;
                        }
                        else
                        {
                            batch.Quantity -= requestQuantity;
                            requestQuantity = 0;
                            if (batch.Quantity == 0)
                            {
                                batch.IsActive = false;
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                product.Stock -= item.Quantity;
            }


            await dbContext.SaveChangesAsync(context.CancellationToken);

            return new UpdateStockResponse
            {
                Success = true,
                Message = "Stock updated successfully."
            };
        }
    }
}


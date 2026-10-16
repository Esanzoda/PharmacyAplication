using Grpc.Contract.Proto;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Models.Product;
using ProductService.CQRS.Product.Mappers;
using ProductService.Exception;
using ProductService.Interfaces;
using CountryEnum = Pharmacy.Domain.Models.Base.Domain.Enum.CountryEnum;
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

        public override async Task<UpdateStockResponse> AddProductBatches(ProductBatches request,
            ServerCallContext context)
        {
            var productIds = request.Branches
                .Select(x => x.ProductId)
                .Distinct()
                .ToList();

            var products = await dbContext.Products
                .Where(x => productIds.Contains(x.Id))
                .ToDictionaryAsync(x => x.Id);

            var batches = new List<ProductBatch>();

            foreach (var branch in request.Branches)
            {
                if (!products.TryGetValue(branch.ProductId, out var product))
                {
                    return new UpdateStockResponse
                    {
                        Success = false,
                        Message = "Product not found"
                    };
                }

                if (branch.Quantity <= 0)
                {
                    return new UpdateStockResponse
                    {
                        Success = false,
                        Message = "Quantity must be greater than zero."
                    };
                }

                var productionDate = new System.DateOnly(
                    branch.ProductionDate.Year,
                    branch.ProductionDate.Month,
                    branch.ProductionDate.Day);

                var expiryDate = new System.DateOnly(
                    branch.ExpiryDate.Year,
                    branch.ExpiryDate.Month,
                    branch.ExpiryDate.Day);

                var batch = new ProductBatch
                {
                    ProductEntityId = branch.ProductId,
                    PharmacyId = branch.PharmacyId,
                    PurchaseItemId = branch.PurchaseItemId,
                    Quantity = branch.Quantity,
                    Country = (CountryEnum)branch.Country,
                    ProductionDate = productionDate,
                    ExpiryDate = expiryDate,
                    PurchasePrice = (decimal)branch.PurchasePrice,
                    TotalPurchasePrice = (decimal)branch.TotalPrice,
                    IsActive = true,
                    Name = product.Name,
                };

                batches.Add(batch);
                product.Stock += batch.Quantity;
            }

            await dbContext.ProductBatches.AddRangeAsync(
                batches,
                context.CancellationToken);

            await dbContext.SaveChangesAsync(context.CancellationToken);

            return new UpdateStockResponse
            {
                Success = true,
                Message = "Product batches added successfully."
            };
        }

        public override async Task<ProductGrpcResponse> GetProductWithIdAndPharmacyId(
            ProductWithIdAndPharmacyIdRequest request, ServerCallContext context)
        {
            var product = await dbContext.Products
                .FirstOrDefaultAsync(x => x.Id == request.Id &&
                                          x.PharmacyId == request.PharmacyId);
            if (product == null)
            {
                throw new ResourceNotFoundException("Product not found");
            }

            return ProductMappers.ToProductGrpcResponse(product);
        }

        public override async Task<UpdateStockResponse> UpdateProductBatch(AddItemToBatchRequest request,
            ServerCallContext context)
        {
            var product = await dbContext.Products
                .FirstOrDefaultAsync(x => x.Id == request.ProductId);
            if (product == null)
            {
                throw new ResourceNotFoundException($"Product not found");
            }

            var productBatch = await dbContext.ProductBatches
                .FirstOrDefaultAsync(x => x.PurchaseItemId == request.PurchaseItemId);
            if (productBatch == null)
            {
                throw new ResourceNotFoundException($"Product Batch not found");
            }

            productBatch.Quantity = request.Quantity;
            productBatch.TotalPurchasePrice = (decimal)request.TotalPrice;
            product.Stock += request.Quantity;
            await dbContext.SaveChangesAsync();
            return new UpdateStockResponse
            {
                Success = true,
                Message = "Product batches updated successfully."
            };
        }

        public override async Task<UpdateStockResponse> DeleteProductBatchesByPurchaseItemIds(
            PurchaseItemIdsRequest request,
            ServerCallContext context)
        {
            var productBatches = await dbContext.ProductBatches
                .Where(x => request.PurchaseItemId.Contains(x.PurchaseItemId))
                .ToListAsync();
            if (productBatches.Count != request.PurchaseItemId.Count)
            {
                throw new BusinessException("One or more product batch not found");
            }

            var productIds = productBatches
                .Select(x => x.ProductEntityId)
                .ToList();
            var products = await dbContext.Products
                .Where(x => productIds.Contains(x.Id))
                .ToDictionaryAsync(x => x.Id);

            foreach (var batch in productBatches)
            {
                if (!products.TryGetValue(batch.ProductEntityId, out var product))
                {
                    throw new ResourceNotFoundException("Product not found");
                }

                batch.IsActive = false;
                batch.IsDeleted = true;
                product.Stock -= batch.Quantity;
            }

            await dbContext.SaveChangesAsync();
            return new UpdateStockResponse
            {
                Success = true,
                Message = "Product successfully update stock.."
            };
        }

        public override async Task<UpdateStockResponse> DeleteProductBatchByPurchaseId(PurchaseItemIdRequest request,
            ServerCallContext context)
        {
            var productBatch = await dbContext.ProductBatches
                .FirstOrDefaultAsync(x => x.PurchaseItemId == request.PurchaseItemId);
            if (productBatch == null)
            {
                throw new ResourceNotFoundException("Product batch not found");
            }

            var product = await dbContext.Products
                .FirstOrDefaultAsync(x => x.Id == productBatch.ProductEntityId);
            if (product == null)
            {
                throw new ResourceNotFoundException("Product not found");
            }

            productBatch.IsActive = false;
            dbContext.ProductBatches.Remove(productBatch);
            product.Stock -= productBatch.Quantity;

            await dbContext.SaveChangesAsync();
            return new UpdateStockResponse
            {
                Success = true,
                Message = "Product successfully update stock."
            };
        }
    }
}
using Grpc.Contract.Proto;
using MediatR;
using Pharmacy.CQRS.Purchase.Mapper;
using Pharmacy.Domain.Models.PurchaseEntity.DTOs.Request;
using Pharmacy.Domain.Models.PurchaseEntity.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Purchase.Commands;

public record CreatePurchaseCommand(
    long PharmacyId,
    long EmployeeId,
    PurchaseRequest Request) : IRequest<PurchaseResponse>;

public class CreatePurchaseCommandHandler(
    IApplicationDbContext dbContext,
    ProductService.ProductServiceClient productClient) : IRequestHandler<CreatePurchaseCommand, PurchaseResponse>
{
    public async Task<PurchaseResponse> Handle(
        CreatePurchaseCommand request,
        CancellationToken cancellationToken)
    {
        var purchase = new Domain.Models.PurchaseEntity.Purchase
        {
            PharmacyId = request.PharmacyId,
            EmployeeEntityId = request.EmployeeId,
            SupplierId = request.Request.SupplierId
        };

        await dbContext.Purchases
            .AddAsync(purchase, cancellationToken);

        var productIds = request.Request.PurchaseItems
            .Select(x => x.ProductEntityId)
            .Distinct()
            .ToList();
        var response = await productClient.GetProductsAsync(
            new ProductGrpcIdsRequest
            {
                ProductIds = { productIds }
            },
            cancellationToken: cancellationToken);

        var products = response.Products
            .ToDictionary(x => x.ProductId);

        foreach (var item in request.Request.PurchaseItems)
        {
            if (!products.TryGetValue(item.ProductEntityId, out var product))
            {
                throw new ResourceNotFoundException("Product not found");
            }

            var purchaseItem = PurchaseMappers.ToPurchaseItem(item);
            purchaseItem.PharmacyId = request.PharmacyId;
            purchaseItem.ProductEntityId = product.ProductId;
            purchaseItem.PurchaseEntity = purchase;
            purchaseItem.TotalPrice = item.Quantity * item.PurchasePrice;

            purchase.PurchaseItems.Add(purchaseItem);
        }

        purchase.TotalAmount = purchase.PurchaseItems.Sum(x => x.TotalPrice);

        await dbContext.SaveChangesAsync(cancellationToken);
        var branches = new List<AddProductBatch>();
        foreach (var item in purchase.PurchaseItems)
        {
            if (!products.TryGetValue(item.ProductEntityId, out var product))
            {
                throw new ResourceNotFoundException("Product not found");
            }

            branches.Add(new AddProductBatch
            {
                PharmacyId = request.PharmacyId,
                ProductId = product.ProductId,
                Country = (Country)item.Country,
                Quantity = item.Quantity,
                PurchaseItemId = item.Id,
                ProductionDate = new Grpc.Contract.Proto.DateOnly
                {
                    Year = item.ProductionDate.Year,
                    Month = item.ProductionDate.Month,
                    Day = item.ProductionDate.Day
                },

                ExpiryDate = new Grpc.Contract.Proto.DateOnly
                {
                    Year = item.ExpiryDate.Year,
                    Month = item.ExpiryDate.Month,
                    Day = item.ExpiryDate.Day
                },

                PurchasePrice = (double)item.PurchasePrice,
                TotalPrice = (double)item.TotalPrice
            });
        }

        var updateStockResponse = await productClient.AddProductBatchesAsync(
            new ProductBatches
            {
                Branches = { branches }
            },
            cancellationToken: cancellationToken);

        if (!updateStockResponse.Success)
        {
            throw new BusinessException(updateStockResponse.Message);
        }

        return PurchaseMappers.ToPurchaseResponse(purchase);
    }
}
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Interfaces;
using Pharmacy.Models.Domain;

namespace Pharmacy.CQRS.ExpiredProducts.Commands;

public record CreateExpiredProductsCommand : IRequest;

public class CheckExpiredProductsHandler(
    IApplicationDbContext dbContext,
    ILogger<CheckExpiredProductsHandler> logger
) : IRequestHandler<CreateExpiredProductsCommand>
{
    public async Task Handle(CreateExpiredProductsCommand request,
        CancellationToken cancellationToken)
    {
        var today = DateTime.UtcNow.Date;

        var expiredProducts = await dbContext.Products
            .Where(x => x.ExpiryDate.Date <= today && x.Stock > 0)
            .ToListAsync(cancellationToken);

        if (!expiredProducts.Any())
            return;

        var report = new ExpiryDate();

        foreach (var product in expiredProducts)
        {
            var item = new ExpiryDateItems
            {
                PharmacyId = product.PharmacyId,
                Product = product,
                TotalSalePrice = product.Stock * product.SalePrice,
                TotalPurchasePrice = product.Stock * product.PurchasePrice
            };

            report.ExpiryDateItemsList.Add(item);

            report.TotalSalePrice += item.TotalSalePrice;
            report.TotalPurchasePrice += item.TotalPurchasePrice;

            product.Stock = 0;
        }

        await dbContext.ExpireDateProducts.AddAsync(report, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Created expired products report with {Count} items.",
            report.ExpiryDateItemsList.Count);
    }
}
using Pharmacy.CQRS.Product.ProductModels;
using Pharmacy.CQRS.Product.ProductModels.DTos.Request;
using Pharmacy.CQRS.Product.ProductModels.DTos.Response;
using Pharmacy.CQRS.Product.ProductModels.DTos.Response.Customer;

namespace Pharmacy.CQRS.Product.Mapper;

public static class ProductMappers
{
    public static ProductEntity ToProduct(ProductRequest request)
    {
        return new ProductEntity
        {
            Name = request.Name,
            ProductType = request.ProductType,
            CategoryEntityId = request.CategoryId,
            Description = request.Description,
            SalePrice = request.SalePrice,
            Barcode = request.Barcode
        };
    }

    public static ProductEntity ToProduct(UpdateProductRequest request)
    {
        return new ProductEntity
        {
            Name = request.Name,
            ProductType = request.ProductType,
            CategoryEntityId = request.CategoryId,
            Description = request.Description
        };
    }

    private static ProductBatchResponse ToProductBatchForPharmacyResponse(ProductBatch productBatch)
    {
        return new ProductBatchResponse
        {
            Id = productBatch.Id,
            Quantity = productBatch.Quantity,
            Country = productBatch.Country,
            ProductionDate = productBatch.ProductionDate,
            ExpiryDate = productBatch.ExpiryDate,
            PurchasePrice = productBatch.PurchasePrice
        };
    }

    public static ProductForPharmacyResponse ToProductForPharmacyResponse(ProductEntity product)
    {
        return new ProductForPharmacyResponse
        {
            Id = product.Id,
            PharmacyId = product.PharmacyId,
            Name = product.Name,
            Description = product.Description,
            CategoryId = product.CategoryEntityId,
            Stock = product.Stock,
            SalePrice = product.SalePrice,
            Barcode = product.Barcode,
            ProductType = product.ProductType,
            ProductBatchResponses = product.ProductBatches
                .Select(ToProductBatchForPharmacyResponse)
                .ToList()
        };
    }

    public static List<ProductForPharmacyResponse> ToListProductForPharmacyResponse(List<ProductEntity> products)
    {
        return products
            .Select(ToProductForPharmacyResponse)
            .ToList();
    }


    private static ProductBatchForCustomerResponse ToProductBatchForCustomerResponse(ProductBatch productBatch)
    {
        return new ProductBatchForCustomerResponse
        {
            Quantity = productBatch.Quantity,
            Country = productBatch.Country,
            ProductionDate = productBatch.ProductionDate,
            ExpiryDate = productBatch.ExpiryDate
        };
    }

    public static ProductForCustomerResponse ToProductForCustomerResponse(ProductEntity productEntity)
    {
        return new ProductForCustomerResponse
        {
            Id = productEntity.Id,
            PharmacyId = productEntity.PharmacyId,
            Name = productEntity.Name,
            Description = productEntity.Description,
            CategoryEntityId = productEntity.CategoryEntityId,
            Stock = productEntity.Stock,
            SalePrice = productEntity.SalePrice,
            ProductType = productEntity.ProductType,
            Barcode = productEntity.Barcode,
            ProductBatchForCustomerResponses = productEntity.ProductBatches
                .Select(ToProductBatchForCustomerResponse)
                .ToList()
        };
    }

    public static List<ProductForCustomerResponse> ToListProductForCustomerResponse(List<ProductEntity> products)
    {
        return products
            .Select(ToProductForCustomerResponse)
            .ToList();
    }
}
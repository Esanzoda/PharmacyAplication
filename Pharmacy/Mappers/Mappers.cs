using AutoMapper;
using Pharmacy.CQRS.Cart.Models;
using Pharmacy.CQRS.Cart.Models.DTOs.Request;
using Pharmacy.CQRS.Cart.Models.DTOs.Response;
using Pharmacy.CQRS.Category.Models;
using Pharmacy.CQRS.Category.Models.DTOs.Request;
using Pharmacy.CQRS.Category.Models.DTOs.Response;
using Pharmacy.CQRS.Customer.Models;
using Pharmacy.CQRS.Customer.Models.DTOs.Request;
using Pharmacy.CQRS.Customer.Models.DTOs.Response;
using Pharmacy.CQRS.Deliver.Models;
using Pharmacy.CQRS.Deliver.Models.DTOs.Request;
using Pharmacy.CQRS.Deliver.Models.DTOs.Response;
using Pharmacy.CQRS.Employee.Models;
using Pharmacy.CQRS.Employee.Models.DTOs.Request;
using Pharmacy.CQRS.Employee.Models.DTOs.Response;
using Pharmacy.CQRS.ExpiredProducts.Models;
using Pharmacy.CQRS.Order.Models;
using Pharmacy.CQRS.Order.Models.DTOs.Request;
using Pharmacy.CQRS.Order.Models.DTOs.Response;
using Pharmacy.CQRS.Pharmacy.Models;
using Pharmacy.CQRS.Pharmacy.Models.DTOs.Request;
using Pharmacy.CQRS.Pharmacy.Models.DTOs.Response;
using Pharmacy.CQRS.Product.ProductModels;
using Pharmacy.CQRS.Product.ProductModels.DTos.Request;
using Pharmacy.CQRS.Product.ProductModels.DTos.Response;
using Pharmacy.CQRS.Product.ProductModels.DTos.Response.Customer;
using Pharmacy.CQRS.Purchase.Models;
using Pharmacy.CQRS.Purchase.Models.DTOs.Request;
using Pharmacy.CQRS.Purchase.Models.DTOs.Response;

namespace Pharmacy.Mappers;

public class Mappers : Profile
{
    public Mappers()
    {
        CreateMap<CartEntity, CartResponse>()
            .ForMember(x => x.CartItemResponse,
                x => x.MapFrom(o => o.CartItems));
        CreateMap<CartItemRequest, CartItemEntity>()
            .ForMember(x => x.CartEntity, x => x.Ignore())
            .ForMember(x => x.CustomerEntityId, x => x.Ignore())
            .ForMember(x => x.SalePrice, x => x.Ignore())
            .ForMember(x => x.TotalPrice, x => x.Ignore());
        CreateMap<CartItemEntity, CartItemResponse>();

        CreateMap<OrderRequest, OrderEntity>()
            .ForMember(x => x.OrderItems, opt => opt.Ignore());

        CreateMap<OrderItemRequest, OrderItemEntity>()
            .ForMember(x => x.Price, opt => opt.Ignore())
            .ForMember(x => x.TotalPrice, opt => opt.Ignore());
        CreateMap<OrderItemEntity, OrderItemResponse>();
        CreateMap<OrderEntity, OrderResponse>()
            .ForMember(x => x.OrderItemResponses,
                x => x.MapFrom(o => o.OrderItems));
        CreateMap<OrderEntity, OrderResponseForDeliver>()
            .ForMember(x => x.Deliver,
                opt => opt.MapFrom(src => src.Deliver)
            );
        CreateMap<DeliverEntity, DeliverResponseForOrder>();


        CreateMap<OrderEntity, OrderResponseForCustomer>()
            .ForMember(x => x.OrderItemResponses,
                x => x.MapFrom(o => o.OrderItems));


        CreateMap<PurchaseRequest, Purchase>()
            .ForMember(x => x.PurchaseItems, opt => opt.Ignore());
        CreateMap<PurchaseItemRequest, PurchaseItem>();
        CreateMap<PurchaseItem, PurchaseItemResponse>();
        CreateMap<Purchase, PurchaseResponse>()
            .ForMember(x => x.PurchaseItems,
                x => x.MapFrom(y => y.PurchaseItems));

        CreateMap<CreateCategoryRequest, CategoryEntity>()
            .ForMember(x => x.CategoryStatus, opt => opt.Ignore());
        CreateMap<CategoryEntity, CategoryResponse>();
        CreateMap<UpdateCategoryRequest, CategoryEntity>();
        CreateMap<CategoryEntity, UpdateCategoryResponse>();


        CreateMap<CustomerRequest, CustomerEntity>()
            .ForMember(x => x.PasswordHash, opt => opt.Ignore())
            .ForMember(x => x.Latitude, opt => opt.Ignore())
            .ForMember(x => x.Longitude, opt => opt.Ignore());
        CreateMap<CustomerEntity, CustomerResponse>();
        CreateMap<UpdateCustomerRequest, CustomerEntity>()
            .ForMember(x => x.Latitude, opt => opt.Ignore())
            .ForMember(x => x.Longitude, opt => opt.Ignore());


        CreateMap<EmployeeRequest, EmployeeEntity>()
            .ForMember(x => x.PasswordHash, opt => opt.Ignore())
            .ForMember(x => x.PharmacyId, opt => opt.Ignore());
        CreateMap<EmployeeEntity, EmployeeResponse>();
        CreateMap<UpdateEmployeeRequest, EmployeeEntity>();


        CreateMap<ProductRequest, ProductEntity>()
            .ForMember(x => x.PharmacyId, opt => opt.Ignore())
            .ForMember(x => x.Stock, opt => opt.Ignore());
        CreateMap<ProductBatch, ProductBatchForCustomerResponse>();
        CreateMap<ProductEntity, ProductForCustomerResponse>()
            .ForMember(x => x.ProductBatchForCustomerResponses,
                x => x.MapFrom(p => p.ProductBatches));
        CreateMap<ProductBatch, ProductBatchResponse>();
        CreateMap<ProductEntity, ProductForPharmacyResponse>();


        CreateMap<UpdateProductRequest, ProductEntity>();
        CreateMap<ProductEntity, ProductForPharmacyResponse>()
            .ForMember(x => x.ProductBatchResponses,
                x => x.MapFrom(o => o.ProductBatches));

        CreateMap<DeliverRequest, DeliverEntity>()
            .ForMember(x => x.Shot, opt => opt.Ignore())
            .ForMember(x => x.PasswordHash, opt => opt.Ignore());
        CreateMap<DeliverEntity, DeliverResponse>();
        CreateMap<UpdateDeliverRequest, DeliverEntity>()
            .ForMember(x => x.Shot, opt => opt.Ignore())
            .ForMember(x => x.PasswordHash, opt => opt.Ignore());


        CreateMap<PharmacyRequest, PharmacyEntity>()
            .ForMember(x => x.Latitude, opt => opt.Ignore())
            .ForMember(x => x.Longitude, opt => opt.Ignore());
        CreateMap<PharmacyEntity, PharmacyResponse>();

        CreateMap<ExpiryDateEntity, ExpireDateProductResponse>()
            .ForMember(x => x.ExpiryDateItemsListResponse,
                x => x.MapFrom(o => o.ExpiryDateItemsList));
    }
}
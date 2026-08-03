using AutoMapper;
using Pharmacy.Controllers;
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
using Pharmacy.CQRS.Employee.Models;
using Pharmacy.CQRS.Employee.Models.DTOs.Request;
using Pharmacy.CQRS.Employee.Models.DTOs.Response;
using Pharmacy.CQRS.Order.Models;
using Pharmacy.CQRS.Order.Models.DTOs.Request;
using Pharmacy.CQRS.Order.Models.DTOs.Response;
using Pharmacy.CQRS.Pharmacy.Models.DTOs.Request;
using Pharmacy.CQRS.Product.ProductModels;
using Pharmacy.CQRS.Product.ProductModels.DTos.Request;
using Pharmacy.CQRS.Product.ProductModels.DTos.Response;
using Pharmacy.CQRS.Purchase.Models;
using Pharmacy.CQRS.Purchase.Models.DTOs.Request;
using Pharmacy.CQRS.Purchase.Models.DTOs.Response;
using PharmacyResponse = Pharmacy.CQRS.Deliver.Models.DTOs.Response.PharmacyResponse;

namespace Pharmacy.Mappers;

public class Mappers : Profile
{
    public Mappers()
    {
        CreateMap<Cart, CartResponse>()
            .ForMember(x => x.CartItemResponse,
                x => x.MapFrom(o => o.CartItems));
        CreateMap<CartItemRequest, CartItem>()
            .ForMember(x => x.Cart, x => x.Ignore())
            .ForMember(x => x.CustomerId, x => x.Ignore())
            .ForMember(x => x.Price, x => x.Ignore())
            .ForMember(x => x.TotalPrice, x => x.Ignore());
        CreateMap<CartItem, CartItemResponse>();

        CreateMap<OrderRequest, Order>()
            .ForMember(x => x.OrderItems, opt => opt.Ignore());

        CreateMap<OrderItemRequest, OrderItem>()
            .ForMember(x => x.Price, opt => opt.Ignore())
            .ForMember(x => x.TotalPrice, opt => opt.Ignore());
        CreateMap<OrderItem, OrderItemResponse>();
        CreateMap<Order, OrderResponse>()
            .ForMember(x => x.OrderItemResponses,
                x => x.MapFrom(o => o.OrderItems));
        CreateMap<Order, OrderResponseForCustomer>()
            .ForMember(x => x.OrderItemResponses,
                x => x.MapFrom(o => o.OrderItems));


        CreateMap<PurchaseRequest, Purchase>()
            .ForMember(x => x.PurchaseItems, opt => opt.Ignore());
        CreateMap<PurchaseItemRequest, PurchaseItem>();
        CreateMap<PurchaseItem, PurchaseItemResponse>();
        CreateMap<Purchase, PurchaseResponse>()
            .ForMember(x => x.PurchaseItems, x => x.MapFrom(y => y.PurchaseItems));

        CreateMap<CreateCategoryRequest, Category>()
            .ForMember(x => x.CategoryStatus, opt => opt.Ignore());
        CreateMap<Category, CategoryResponse>();
        CreateMap<UpdateCategoryRequest, Category>();
        CreateMap<Category, UpdateCategoryResponse>();


        CreateMap<CustomerRequest, Customer>()
            .ForMember(x => x.PasswordHash, opt => opt.Ignore())
            .ForMember(x => x.Latitude, opt => opt.Ignore())
            .ForMember(x => x.Longitude, opt => opt.Ignore());
        CreateMap<Customer, CustomerResponse>();
        CreateMap<UpdateCustomerRequest, Customer>()
            .ForMember(x => x.Latitude, opt => opt.Ignore())
            .ForMember(x => x.Longitude, opt => opt.Ignore());


        CreateMap<EmployeeRequest, Employee>()
            .ForMember(x => x.PasswordHash, opt => opt.Ignore())
            .ForMember(x => x.PharmacyId, opt => opt.Ignore());
        CreateMap<Employee, EmployeeResponse>();


        CreateMap<ProductRequest, Product>()
            .ForMember(x => x.PharmacyId, opt => opt.Ignore());
        CreateMap<Product, ProductResponse>();

        CreateMap<DeliverRequest, Deliver>()
            .ForMember(x => x.Shot, opt => opt.Ignore())
            .ForMember(x => x.PasswordHash, opt => opt.Ignore());
        CreateMap<Deliver, DeliverResponse>();
        CreateMap<UpdateDeliverRequest, Deliver>()
            .ForMember(x => x.Shot, opt => opt.Ignore())
            .ForMember(x => x.PasswordHash, opt => opt.Ignore());


        CreateMap<PharmacyRequest, CQRS.Pharmacy.Models.Pharmacy>()
            .ForMember(x => x.Latitude, opt => opt.Ignore())
            .ForMember(x => x.Longitude, opt => opt.Ignore());
        CreateMap<CQRS.Pharmacy.Models.Pharmacy, PharmacyResponse>();
    }
}
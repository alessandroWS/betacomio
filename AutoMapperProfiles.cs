using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using betacomio.Dtos.OldOrder;
using betacomio.Dtos.Product;

namespace betacomio
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Order, GetOrderDto>();
            CreateMap<AddOrderDto, Order>();
            CreateMap<Product, GetProductDto>();
            CreateMap<AddProductDto, Product>();
             CreateMap<SalesOrderHeader, OldOrderDto>()
            .ForMember(dest => dest.CustomerID, opt => opt.MapFrom(src => src.Customer.CustomerId))
            .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.Customer.EmailAddress))
            .ForMember(dest => dest.SalesOrderID, opt => opt.MapFrom(src => src.SalesOrderId))
            .ForMember(dest => dest.SalesOrderDetailID, opt => opt.MapFrom(src => src.SalesOrderDetails.FirstOrDefault().SalesOrderDetailId))
            .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate))
            .ForMember(dest => dest.OrderQty, opt => opt.MapFrom(src => src.SalesOrderDetails.FirstOrDefault().OrderQty))
            .ForMember(dest => dest.ProductID, opt => opt.MapFrom(src => src.SalesOrderDetails.FirstOrDefault().ProductId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.SalesOrderDetails.FirstOrDefault().Product.Name));
    
    
        }
    }
}
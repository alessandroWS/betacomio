namespace betacomio
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // Definisce una mappatura tra la classe Order e la classe GetOrderDto
            CreateMap<Order, GetOrderDto>();

            // Definisce una mappatura tra la classe AddOrderDto e la classe Order
            CreateMap<AddOrderDto, Order>();

            // Definisce una mappatura tra la classe Product e la classe GetProductDto
            CreateMap<Product, GetProductDto>();

            // Definisce una mappatura tra la classe AddProductDto e la classe Product
            CreateMap<AddProductDto, Product>();

            // Definisce una mappatura personalizzata per la classe SalesOrderHeader e la classe OldOrderDto
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

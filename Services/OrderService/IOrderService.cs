using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace betacomio.Services.OrderService
{
    public interface IOrderService
    {
        Task<ServiceResponse<List<GetOrderDto>>> GetAllOrder();
        Task<ServiceResponse<GetOrderDto>> GetOrderById(int id);
        Task<ServiceResponse<List<GetOrderDto>>> AddOrder(AddOrderDto newOrder);
        Task<ServiceResponse<GetOrderDto>> UpdateOrder(UpdateOrderDto updatedOrder);
        Task<ServiceResponse<List<GetOrderDto>>> DeleteOrder(int id);
    }
}
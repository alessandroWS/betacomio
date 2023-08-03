global using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using betacomio.Models;

namespace betacomio.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private static List<Order> orders = new List<Order>{
            new Order(),
            new Order{ Id=1, ProductName="Prodotto"}
        };
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext2 _context2;
        public OrderService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor, DataContext2 context2)
        {
            _context2 = context2;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _context = context;

        }

        
        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User
        .FindFirstValue(ClaimTypes.NameIdentifier)!);

        public async Task<ServiceResponse<List<GetOrderDto>>> AddOrder(AddOrderDto newOrder)
        {
            var serviceResponse = new ServiceResponse<List<GetOrderDto>>();
            var order = _mapper.Map<Order>(newOrder);
            
            order.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            serviceResponse.Data = 
            await _context.Orders
            .Where(c=>c.User!.Id==GetUserId())
            .Select(c => _mapper.Map<GetOrderDto>(c))
            .ToListAsync();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetOrderDto>>> DeleteOrder(int id)
        {
            
            var serviceResponse = new ServiceResponse<List<GetOrderDto>>();
            try 
            {
            var order = await _context.Orders
            
            .FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == GetUserId());
            if(order is null)
                throw new Exception($"order with id {id} not found");    
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            serviceResponse.Data = 
            await _context.Orders
            .Where(c => c.User!.Id == GetUserId())
            .Select(c => _mapper.Map<GetOrderDto>(c)).ToListAsync(); 
            }
            catch (Exception ex)
            {
                serviceResponse.Success=false;
                serviceResponse.Message=ex.Message;

            }
            
        return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetOrderDto>>> GetAllOrder()
        {
           var serviceResponse = new ServiceResponse<List<GetOrderDto>>();
           var dbOrder= await _context.Orders.Where(c=>c.User!.Id==GetUserId()).ToListAsync();
            serviceResponse.Data = dbOrder.Select(c => _mapper.Map<GetOrderDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetOrderDto>> GetOrderById(int id)
        {
            var serviceResponse = new ServiceResponse<GetOrderDto>();
            var dbOrder = await _context.Orders
            .FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == GetUserId());
            serviceResponse.Data = _mapper.Map<GetOrderDto>(dbOrder);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetOrderDto>> UpdateOrder(UpdateOrderDto updatedOrder)
        {
            
            var serviceResponse = new ServiceResponse<GetOrderDto>();
            try 
            {
            var order = 
            await _context.Orders
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == updatedOrder.Id);
            if(order is null || order.User!.Id != GetUserId())
                throw new Exception($"order with id {updatedOrder.Id} not found");    
            order.ProductName = updatedOrder.ProductName;
            order.Quantity = updatedOrder.Quantity;
            order.Price = updatedOrder.Price;
            order.Phone = updatedOrder.Phone;
            await _context.SaveChangesAsync();
            serviceResponse.Data = _mapper.Map<GetOrderDto>(order); 
   
            }
            catch (Exception ex)
            {
                serviceResponse.Success=false;
                serviceResponse.Message=ex.Message;

            }
            
        return serviceResponse;
        }
    }
}
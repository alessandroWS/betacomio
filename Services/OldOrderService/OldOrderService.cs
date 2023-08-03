using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using betacomio.Models;

namespace betacomio.Services.OldOrderService
{
    public class OldOrderService : IOldOrderService
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly AdventureWorksLt2019Context _adventure;
        private readonly IMapper _mapper;

        public OldOrderService(IHttpContextAccessor httpContext, AdventureWorksLt2019Context adventure, IMapper mapper)
        {
            _httpContext = httpContext;
            _adventure = adventure;
            _mapper = mapper;
        }
        private string GetUserName() => _httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.Name)!;
       public async Task<ServiceResponse<List<OldOrderDto>>> GetAllOldOrderForCustomer()
        {

        //     var serviceResponse = new ServiceResponse<List<GetOrderDto>>();
        //     var dbOrder= await _context.Orders.Where(c=>c.User!.Id==GetUserId()).ToListAsync();
        //     serviceResponse.Data = dbOrder.Select(c => _mapper.Map<GetOrderDto>(c)).ToList();
        //     return serviceResponse;

            var serviceResponse = new ServiceResponse<List<OldOrderDto>>();

            // Continua con il resto del codice come prima
            var dbOldOrders = await _adventure.SalesOrderHeaders
            .Where(soh => soh.Customer.EmailAddress == GetUserName())
            .SelectMany(soh => soh.SalesOrderDetails, (soh, sod) => new { soh, sod })
            .Select(x => new OldOrderDto
            {
                CustomerID = x.soh.CustomerId,
                EmailAddress = x.soh.Customer.EmailAddress,
                SalesOrderID = x.soh.SalesOrderId,
                SalesOrderDetailID = x.sod.SalesOrderDetailId,
                OrderDate = x.soh.OrderDate,
                OrderQty = x.sod.OrderQty,
                ProductID = x.sod.ProductId,
                Name = x.sod.Product.Name
            })
            .ToListAsync();

            serviceResponse.Data = dbOldOrders.Select(c => _mapper.Map<OldOrderDto>(c)).ToList();
            // ...

            return serviceResponse;
        }


    }
}

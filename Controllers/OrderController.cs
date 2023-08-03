using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using betacomio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace betacomio.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<GetOrderDto>>>> Get()
        {
            
            int userId = int.Parse(User.Claims.FirstOrDefault(c=>c.Type == ClaimTypes.NameIdentifier)!.Value);
            return Ok(await _orderService.GetAllOrder());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetOrderDto>>> GetSingle(int id)
        {
            return Ok(await _orderService.GetOrderById(id));
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<GetOrderDto>>>> AddOrder(AddOrderDto newOrder)
        {
            return Ok(await _orderService.AddOrder(newOrder));
        }
        
        [HttpPut]
        public async Task<ActionResult<ServiceResponse<List<GetOrderDto>>>> UpdateOrder(UpdateOrderDto updatedOrder)
        {
            var response = await _orderService.UpdateOrder(updatedOrder);
            if (response.Data is null)
                return NotFound(response);
            return Ok(response);
        }

         [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<GetOrderDto>>> DeleteOrder(int id)
        {
            var response = await _orderService.DeleteOrder(id);
            if (response.Data is null)
                return NotFound(response);
            return Ok(response);
        }
    }
}
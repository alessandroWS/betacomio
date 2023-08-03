using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using betacomio.Dtos.Product;
using betacomio.Services.OldOrderService;
using betacomio.Dtos.OldOrder;
using Microsoft.AspNetCore.Authorization;

namespace betacomio.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OldOrderController : ControllerBase
    {
        private readonly IOldOrderService _oldOrderService;

        public OldOrderController(IOldOrderService oldOrderService)
        {
            _oldOrderService = oldOrderService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<OldOrderDto>>>> Get()
        {
            string userName = User.Claims.FirstOrDefault(c=>c.Type == ClaimTypes.Name)!.Value.ToString();
            return Ok(await _oldOrderService.GetAllOldOrderForCustomer());
    }
}}

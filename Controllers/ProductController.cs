using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using betacomio.Services.ProductServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace betacomio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController (IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<GetOrderDto>>>> Get()
        {
            
            return Ok(await _productService.GetAllProduct());
        }
    }
}
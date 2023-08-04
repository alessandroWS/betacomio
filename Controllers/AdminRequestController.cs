using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using betacomio.Dtos.User;
using betacomio.Services.AdminRequestService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace betacomio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminRequestController : ControllerBase
    {
        private readonly IAdminRequestService _requestservice;

        public AdminRequestController(IAdminRequestService requestservice)
        {
            _requestservice = requestservice;
        }


        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<GetOrderDto>>>> Get()
        {
            return Ok(await _requestservice.GetAllRequestAdmin());
        }
    }
}

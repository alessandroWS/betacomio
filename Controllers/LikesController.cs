using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using betacomio.Dtos.Likes;
using betacomio.Services.LikesService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace betacomio.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class LikesController : Controller
    {
        private readonly ILikesService _likeService;

        public LikesController(ILikesService likeService)
        {
            _likeService = likeService;
        }

        [HttpPost("AddLike")]
        public async Task<ActionResult<ServiceResponse<Like>>> AddLikes(AddLikesDto addlikedto)
        {        
            var response = await _likeService.AddLikes(addlikedto);
            return response.Success ? Ok(response) : BadRequest(response.Message);
        }

        [HttpGet("GetAllLike")]
        public async Task<ActionResult<ServiceResponse<List<AddLikesDto>>>> Get()
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value);

            var orders = await _likeService.GetAllLikes(userId);

            return Ok(orders);
        }
    }
}

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

        // Costruttore della classe che richiede una dipendenza dell'interfaccia IOrderService
        public LikesController(ILikesService likeService)
        {
            _likeService = likeService;
        }


        [HttpPost] // Assicurati di aver configurato l'autenticazione nel file di configurazione (Startup.cs)
        public async Task<ActionResult<ServiceResponse<Like>>> AddLikes(AddLikesDto addlikedto)
        {        
            var response = await _likeService.AddLikes(addlikedto);
            return response.Success ? Ok(response) : BadRequest(response.Message);
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<AddLikesDto>>>> Get()
        {
            // // Ottiene l'Id dell'utente autenticato dalla claims
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value);

            // Chiama il servizio IOrderService per ottenere tutti gli ordini per l'utente
            var orders = await _likeService.GetAllLikes(userId);

            // Restituisce una risposta HTTP con lo status 200 (OK) e i dati degli ordini
            return Ok(orders);
        }

        // Metodo per ottenere UserId dal Claim del JWT
       
    }
}
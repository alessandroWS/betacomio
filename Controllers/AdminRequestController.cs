using System.Collections.Generic;
using System.Threading.Tasks;
using betacomio.Services.AdminRequestService;
using Microsoft.AspNetCore.Mvc;
using betacomio.Dtos.OldOrder;
using betacomio.Dtos.AdminRequest;

namespace betacomio.Controllers
{
    
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AdminRequestController : ControllerBase
    {
        private readonly IAdminRequestService _requestservice;

        public AdminRequestController(IAdminRequestService requestservice, IMapper mapper)
        {
            _requestservice = requestservice;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<AdminRequest>>>> Get()
        {
            var adminRequest = await _requestservice.GetAllReq();
            var adminRequestDtos = adminRequest;
            return Ok(adminRequestDtos);
        } 

        [HttpGet("GetAllCount")]
        public async Task<ServiceResponse<int>> GetCount()
        {
            var adminRequest = await _requestservice.GetAllReqCount();
            var adminRequestDtos = adminRequest;
            return adminRequestDtos;
        } 

        [HttpPut] // Attributo per specificare il percorso dell'endpoint di questo metodo con il metodo HTTP PUT
        public async Task<ActionResult<ServiceResponse<List<PutReqDto>>>> UpdateReq(int id, PutReqDto putDto)
        {
            // Chiama il servizio IOrderService per aggiornare l'ordine per l'utente autenticato
            var response = await _requestservice.UpdateReq(id, putDto);

            // Restituisce una risposta HTTP con lo status 200 (OK) e i dati degli ordini aggiornati, se l'ordine è stato trovato
            // Altrimenti, restituisce una risposta HTTP con lo status 404 (Not Found)
            return response.Data is null ? NotFound(response) : Ok(response);
        }
        [HttpPost] // Assicurati di aver configurato l'autenticazione nel file di configurazione (Startup.cs)
        public async Task<ActionResult<ServiceResponse<AdminRequest>>> PostAdminRequest(PostAdminRequestDto postDto)
        {
            int userId = GetUserIdFromClaim(); // Metodo per ottenere UserId dal Claim del JWT
            if (userId == 0)
            {
                return BadRequest("UserId non trovato nel Claim del JWT.");
            }

            var adminRequest = new AdminRequest
            {
                UserId = userId,
                // Gli altri valori saranno i valori di default specificati nella classe AdminRequest
            };

            var response = await _requestservice.CreateAdminRequest(adminRequest);
            return response.Success ? Ok(response) : BadRequest(response.Message);
        }

        // Metodo per ottenere UserId dal Claim del JWT
        private int GetUserIdFromClaim()
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (claim != null && int.TryParse(claim.Value, out int userId))
            {
                return userId;
            }
            return 0;
        }

    }
}

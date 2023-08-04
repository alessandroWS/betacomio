using System.Collections.Generic;
using System.Threading.Tasks;
using betacomio.Services.AdminRequestService;
using Microsoft.AspNetCore.Mvc;
using betacomio.Dtos.OldOrder;
using betacomio.Dtos.AdminRequest;

namespace betacomio.Controllers
{
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
        [HttpPut] // Attributo per specificare il percorso dell'endpoint di questo metodo con il metodo HTTP PUT
        public async Task<ActionResult<ServiceResponse<List<PutReqDto>>>> UpdateReq(int id, PutReqDto putDto)
        {
            // Chiama il servizio IOrderService per aggiornare l'ordine per l'utente autenticato
            var response = await _requestservice.UpdateReq(id, putDto);

            // Restituisce una risposta HTTP con lo status 200 (OK) e i dati degli ordini aggiornati, se l'ordine è stato trovato
            // Altrimenti, restituisce una risposta HTTP con lo status 404 (Not Found)
            return response.Data is null ? NotFound(response) : Ok(response);
        }
    }
}

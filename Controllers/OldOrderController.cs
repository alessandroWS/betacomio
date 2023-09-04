namespace betacomio.Controllers
{
    // Controller API per gestire le operazioni relative agli ordini vecchi
    [Authorize] // Attributo che indica che l'accesso a questo controller richiede l'autorizzazione
    [ApiController] // Attributo che indica che questo è un controller API
    [Route("api/[controller]")] // Attributo per specificare il percorso di base delle richieste per questo controller
    public class OldOrderController : ControllerBase
    {
        private readonly IOldOrderService _oldOrderService;

        private static Logger logger= LogManager.GetCurrentClassLogger();
        // Costruttore della classe che richiede una dipendenza dell'interfaccia IOldOrderService
        public OldOrderController(IOldOrderService oldOrderService)
        {
            _oldOrderService = oldOrderService;
        }

        // Metodo per ottenere tutti gli ordini vecchi per il cliente autenticato
        // Metodo HTTP: GET
        // Percorso: api/OldOrder/GetAll
        [HttpGet("GetAll")] // Attributo per specificare il percorso dell'endpoint di questo metodo
        public async Task<ActionResult<ServiceResponse<List<OldOrderDto>>>> Get()
        {
            // Ottiene l'Username dell'utente autenticato dalla claims
            string userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)!.Value.ToString();

            // Chiama il servizio IOldOrderService per ottenere tutti gli ordini vecchi per il cliente autenticato
            var oldOrders = await _oldOrderService.GetAllOldOrderForCustomer();

            // Restituisce una risposta HTTP con lo status 200 (OK) e i dati degli ordini vecchi
            return Ok(oldOrders);
        }
    }
}

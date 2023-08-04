namespace betacomio.Controllers
{
    // Controller API per gestire le operazioni relative agli ordini
    [Authorize] // Attributo che indica che l'accesso a questo controller richiede l'autorizzazione
    [ApiController] // Attributo che indica che questo è un controller API
    [Route("api/[controller]")] // Attributo per specificare il percorso di base delle richieste per questo controller
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        // Costruttore della classe che richiede una dipendenza dell'interfaccia IOrderService
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // Metodo per ottenere tutti gli ordini per l'utente autenticato
        // Metodo HTTP: GET
        // Percorso: api/Order/GetAll
        [HttpGet("GetAll")] // Attributo per specificare il percorso dell'endpoint di questo metodo
        public async Task<ActionResult<ServiceResponse<List<GetOrderDto>>>> Get()
        {
            // Ottiene l'Id dell'utente autenticato dalla claims
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value);

            // Chiama il servizio IOrderService per ottenere tutti gli ordini per l'utente
            var orders = await _orderService.GetAllOrder();

            // Restituisce una risposta HTTP con lo status 200 (OK) e i dati degli ordini
            return Ok(orders);
        }

        // Metodo per ottenere un singolo ordine per l'utente autenticato
        // Metodo HTTP: GET
        // Percorso: api/Order/{id}
        [HttpGet("{id}")] // Attributo per specificare il percorso dell'endpoint di questo metodo con un parametro "id"
        public async Task<ActionResult<ServiceResponse<GetOrderDto>>> GetSingle(int id)
        {
            // Chiama il servizio IOrderService per ottenere l'ordine con l'Id specificato per l'utente autenticato
            var order = await _orderService.GetOrderById(id);

            // Restituisce una risposta HTTP con lo status 200 (OK) e i dati dell'ordine
            return Ok(order);
        }

        // Metodo per aggiungere un nuovo ordine per l'utente autenticato
        // Metodo HTTP: POST
        // Percorso: api/Order
        [HttpPost] // Attributo per specificare il percorso dell'endpoint di questo metodo con il metodo HTTP POST
        public async Task<ActionResult<ServiceResponse<List<GetOrderDto>>>> AddOrder(AddOrderDto newOrder)
        {
            // Chiama il servizio IOrderService per aggiungere il nuovo ordine per l'utente autenticato
            var result = await _orderService.AddOrder(newOrder);

            // Restituisce una risposta HTTP con lo status 200 (OK) e i dati degli ordini aggiornati
            return Ok(result);
        }
        
        // Metodo per aggiornare un ordine per l'utente autenticato
        // Metodo HTTP: PUT
        // Percorso: api/Order
        [HttpPut] // Attributo per specificare il percorso dell'endpoint di questo metodo con il metodo HTTP PUT
        public async Task<ActionResult<ServiceResponse<List<GetOrderDto>>>> UpdateOrder(UpdateOrderDto updatedOrder)
        {
            // Chiama il servizio IOrderService per aggiornare l'ordine per l'utente autenticato
            var response = await _orderService.UpdateOrder(updatedOrder);

            // Restituisce una risposta HTTP con lo status 200 (OK) e i dati degli ordini aggiornati, se l'ordine è stato trovato
            // Altrimenti, restituisce una risposta HTTP con lo status 404 (Not Found)
            return response.Data is null ? NotFound(response) : Ok(response);
        }

        // Metodo per eliminare un ordine per l'utente autenticato
        // Metodo HTTP: DELETE
        // Percorso: api/Order/{id}
        [HttpDelete("{id}")] // Attributo per specificare il percorso dell'endpoint di questo metodo con un parametro "id" e il metodo HTTP DELETE
        public async Task<ActionResult<ServiceResponse<GetOrderDto>>> DeleteOrder(int id)
        {
            // Chiama il servizio IOrderService per eliminare l'ordine con l'Id specificato per l'utente autenticato
            var response = await _orderService.DeleteOrder(id);

            // Restituisce una risposta HTTP con lo status 200 (OK) e i dati dell'ordine eliminato, se l'ordine è stato trovato
            // Altrimenti, restituisce una risposta HTTP con lo status 404 (Not Found)
            return response.Data is null ? NotFound(response) : Ok(response);
        }
    }
}

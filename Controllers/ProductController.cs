namespace betacomio.Controllers
{
    // Controller API per gestire le operazioni relative ai prodotti
    [ApiController] // Attributo che indica che questo è un controller API
    [Route("api/[controller]")] // Attributo per specificare il percorso di base delle richieste per questo controller
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        // Costruttore della classe che richiede una dipendenza dell'interfaccia IProductService
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // Metodo per ottenere tutti i prodotti
        // Metodo HTTP: GET
        // Percorso: api/Product/GetAll
        [HttpGet("GetAll")] // Attributo per specificare il percorso dell'endpoint di questo metodo
        public async Task<ActionResult<ServiceResponse<List<GetOrderDto>>>> Get()
        {
            // Chiamata al servizio IProductService per ottenere tutti i prodotti
            var products = await _productService.GetAllProduct();

            // Restituisce una risposta HTTP con lo status 200 (OK) e i dati dei prodotti
            return Ok(products);
        }
    }
}

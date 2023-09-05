namespace betacomio.Controllers
{
    // Controller API per gestire le operazioni relative ai prodotti
    
    [ApiController] // Attributo che indica che questo è un controller API
    [Route("api/[controller]")] // Attributo per specificare il percorso di base delle richieste per questo controller
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        private static Logger logger= LogManager.GetCurrentClassLogger();
        // Costruttore della classe che richiede una dipendenza dell'interfaccia IProductService
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // Metodo per ottenere tutti i prodotti
        // Metodo HTTP: GET
        // Percorso: api/Product/GetAll
        [HttpGet("GetAll")] // Attributo per specificare il percorso dell'endpoint di questo metodo
        
        public async Task<ActionResult<ServiceResponse<List<Product>>>> Get()
        {
            // Chiamata al servizio IProductService per ottenere tutti i prodotti
            var products = await _productService.GetAllProduct();

            // Restituisce una risposta HTTP con lo status 200 (OK) e i dati dei prodotti
            return products;
        }

        [HttpGet("{id}")] // Attributo per specificare il percorso dell'endpoint di questo metodo con un parametro "id"
        public async Task<ActionResult<ServiceResponse<Product>>> GetSingle(int id)
        {
            // Chiama il servizio IOrderService per ottenere l'ordine con l'Id specificato per l'utente autenticato
            var order = await _productService.GetProductById(id);

            // Restituisce una risposta HTTP con lo status 200 (OK) e i dati dell'ordine
            return Ok(order);
        }

        [HttpGet("category/{id}")] // Attributo per specificare il percorso dell'endpoint di questo metodo con un parametro "id"
        public async Task<ActionResult<ServiceResponse<Product>>> GetProductOfCategory(int id)
        {
            // Chiama il servizio IOrderService per ottenere l'ordine con l'Id specificato per l'utente autenticato
            var order = await _productService.GetProductByIdCategory(id);

            // Restituisce una risposta HTTP con lo status 200 (OK) e i dati dell'ordine
            return Ok(order);
        }
        [Authorize]
[HttpPost("Add")]
public async Task<ActionResult<ServiceResponse<List<Product>>>> AddProducts([FromBody] AddProductDto newProduct)
{
    var response = await _productService.AddProducts(newProduct);

    if (response.Success)
    {
        // Restituisci una risposta HTTP con lo status 201 (Created) e i dati dei prodotti
        return CreatedAtAction(nameof(GetSingle), new { id = response.Data[0].ProductId }, response);
    }
    else
    {
        // Restituisci una risposta HTTP con lo status 400 (Bad Request) e i dettagli dell'errore
        return BadRequest(response);
    }
}


    }
}

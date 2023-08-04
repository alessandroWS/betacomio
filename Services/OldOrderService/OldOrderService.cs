namespace betacomio.Services.OldOrderService
{
    // Implementa l'interfaccia IOldOrderService per fornire i servizi relativi agli ordini precedenti (OldOrder)
    public class OldOrderService : IOldOrderService
    {
        // Dichiarazione delle dipendenze utilizzate all'interno della classe
        private readonly IHttpContextAccessor _httpContext; // Oggetto per accedere al contesto HTTP
        private readonly AdventureWorksLt2019Context _adventure; // Oggetto per accedere al contesto del database AdventureWorksLT2019
        private readonly IMapper _mapper; // Oggetto AutoMapper per la mappatura degli oggetti

        // Costruttore della classe, viene utilizzato per iniettare le dipendenze necessarie
        public OldOrderService(IHttpContextAccessor httpContext, AdventureWorksLt2019Context adventure, IMapper mapper)
        {
            _httpContext = httpContext; // Inizializza l'oggetto per accedere al contesto HTTP
            _adventure = adventure; // Inizializza l'oggetto per accedere al contesto del database AdventureWorksLT2019
            _mapper = mapper; // Inizializza l'oggetto AutoMapper
        }

        // Metodo privato per ottenere il nome dell'utente dal contesto HTTP utilizzando i claim di sicurezza
        private string GetUserName() => _httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.Name)!;

        // Metodo per ottenere tutti gli ordini precedenti (OldOrder) relativi al cliente corrente e restituire una lista di OldOrderDto
        public async Task<ServiceResponse<List<OldOrderDto>>> GetAllOldOrderForCustomer()
        {
            // Creazione dell'oggetto di risposta del servizio
            var serviceResponse = new ServiceResponse<List<OldOrderDto>>();

            // Ottiene tutti gli ordini precedenti (OldOrder) del cliente corrente dal database AdventureWorksLT2019 utilizzando LINQ
            var dbOldOrders = await _adventure.SalesOrderHeaders
                .Where(soh => soh.Customer.EmailAddress == GetUserName())
                .SelectMany(soh => soh.SalesOrderDetails, (soh, sod) => new { soh, sod })
                .Select(x => new OldOrderDto
                {
                    CustomerID = x.soh.CustomerId,
                    EmailAddress = x.soh.Customer.EmailAddress,
                    SalesOrderID = x.soh.SalesOrderId,
                    SalesOrderDetailID = x.sod.SalesOrderDetailId,
                    OrderDate = x.soh.OrderDate,
                    OrderQty = x.sod.OrderQty,
                    ProductID = x.sod.ProductId,
                    Name = x.sod.Product.Name
                })
                .ToListAsync();

            // Mapping degli ordini precedenti ottenuti a una lista di OldOrderDto utilizzando AutoMapper
            serviceResponse.Data = dbOldOrders.Select(c => _mapper.Map<OldOrderDto>(c)).ToList();

            // Restituzione dell'oggetto di risposta contenente la lista di OldOrderDto
            return serviceResponse;
        }
    }
}

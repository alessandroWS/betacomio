namespace betacomio.Services.ProductServices
{
    // Implementa l'interfaccia IProductService per fornire i servizi relativi ai prodotti
    public class ProductService : IProductService
    {
        // Dichiarazione delle dipendenze utilizzate all'interno della classe
        private readonly IMapper _mapper; // Oggetto AutoMapper per la mappatura degli oggetti
        private readonly IHttpContextAccessor _httpContext; // Oggetto per accedere al contesto HTTP
        private readonly AdventureWorksLt2019Context _adventure; // Oggetto per accedere al contesto del database AdventureWorksLT2019

        private static Logger logger= LogManager.GetCurrentClassLogger();
        // Costruttore della classe, viene utilizzato per iniettare le dipendenze necessarie
        public ProductService(IMapper mapper, IHttpContextAccessor httpContext, AdventureWorksLt2019Context adventure)
        {
            _mapper = mapper; // Inizializza l'oggetto AutoMapper
            _httpContext = httpContext; // Inizializza l'oggetto per accedere al contesto HTTP
            _adventure = adventure; // Inizializza l'oggetto per accedere al contesto del database AdventureWorksLT2019
        }

        // Metodo per aggiungere un nuovo prodotto (non ancora implementato)
        public Task<ServiceResponse<List<GetProductDto>>> AddProduct(AddProductDto newProduct)
        {
            throw new NotImplementedException();
        }

        // Metodo per eliminare un prodotto per ID (non ancora implementato)
        public Task<ServiceResponse<List<GetProductDto>>> DeleteProduct(int id)
        {
            throw new NotImplementedException();
        }

        // Metodo per ottenere tutti i prodotti e restituire una lista di GetProductDto
      public async Task<ServiceResponse<List<Product>>> GetAllProduct()
{
    // Creazione dell'oggetto di risposta del servizio
    var serviceResponse = new ServiceResponse<List<Product>>();

    try
    {
        // Ottenimento di 10 prodotti dal database utilizzando Entity Framework Core (ToListAsync)
        serviceResponse.Data = await _adventure.Products
            .Include(c => c.ProductCategory)

             //.Take(10)  Prendi solo 10 prodotti
            .ToListAsync();

        // Restituzione dell'oggetto di risposta contenente la lista di GetProductDto
        return serviceResponse;
    }
    catch (Exception ex)
    {
        // Se si verifica un'eccezione durante l'aggiornamento dell'ordine, imposta il flag Success su false e aggiunge il messaggio di errore al campo Message dell'oggetto di risposta
        serviceResponse.Success = false;
        serviceResponse.Message = ex.Message;
        
                logger.Trace(ex.InnerException, ex.Message);
        return serviceResponse;
    }
}

        public async Task<ServiceResponse<Product>> GetProductById(int id)
        {
            // Creazione dell'oggetto di risposta del servizio
            var serviceResponse = new ServiceResponse<Product>();
            try
            {
// Ottiene l'ordine dal DataContext in base all'ID e all'ID dell'utente utilizzando LINQ
            var dbOrder = await _adventure.Products
                .FirstOrDefaultAsync(c => c.ProductId == id);

            // Mapping dell'ordine ottenuto a un oggetto GetOrderDto utilizzando AutoMapper
            serviceResponse.Data = dbOrder;
            }
            catch (Exception ex)
            {
                // Se si verifica un'eccezione durante l'aggiornamento dell'ordine, imposta il flag Success su false e aggiunge il messaggio di errore al campo Message dell'oggetto di risposta
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                logger.Trace(ex.InnerException, ex.Message);

            }
            

            // Restituzione dell'oggetto di risposta contenente l'oggetto GetOrderDto
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<Product>>> GetProductByIdCategory(int id)
        {
            var serviceResponse = new ServiceResponse<List<Product>>();
 
        try
        {
        // Ottenimento di 10 prodotti dal database utilizzando Entity Framework Core (ToListAsync)
        serviceResponse.Data = await _adventure.Products
            .Where(c => c.ProductCategory.ProductCategoryId == id).Include(c => c.ProductCategory)


             //.Take(10)  Prendi solo 10 prodotti
            .ToListAsync();

        // Restituzione dell'oggetto di risposta contenente la lista di GetProductDto
        return serviceResponse;
        }
        catch (Exception ex)
        {
            // Se si verifica un'eccezione durante l'aggiornamento dell'ordine, imposta il flag Success su false e aggiunge il messaggio di errore al campo Message dell'oggetto di risposta
            serviceResponse.Success = false;
            serviceResponse.Message = ex.Message;
            
                logger.Trace(ex.InnerException, ex.Message);
            return serviceResponse;
        }
        }
    }
}

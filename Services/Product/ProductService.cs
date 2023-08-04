namespace betacomio.Services.ProductServices
{
    // Implementa l'interfaccia IProductService per fornire i servizi relativi ai prodotti
    public class ProductService : IProductService
    {
        // Dichiarazione delle dipendenze utilizzate all'interno della classe
        private readonly IMapper _mapper; // Oggetto AutoMapper per la mappatura degli oggetti
        private readonly IHttpContextAccessor _httpContext; // Oggetto per accedere al contesto HTTP
        private readonly AdventureWorksLt2019Context _adventure; // Oggetto per accedere al contesto del database AdventureWorksLT2019

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
        public async Task<ServiceResponse<List<GetProductDto>>> GetAllProduct()
        {
            // Creazione dell'oggetto di risposta del servizio
            var serviceResponse = new ServiceResponse<List<GetProductDto>>();

            // Ottenimento di tutti i prodotti dal database utilizzando Entity Framework Core (ToListAsync)
            var dbProducts = await _adventure.Products.ToListAsync();

            // Mapping dei prodotti del database a oggetti GetProductDto utilizzando AutoMapper (Select e Map)
            serviceResponse.Data = dbProducts.Select(product => _mapper.Map<GetProductDto>(product)).ToList();

            // Restituzione dell'oggetto di risposta contenente la lista di GetProductDto
            return serviceResponse;
        }

        // Metodo per ottenere un prodotto per ID (non ancora implementato)
        public Task<ServiceResponse<GetProductDto>> GetProductById(int id)
        {
            throw new NotImplementedException();
        }
    }
}

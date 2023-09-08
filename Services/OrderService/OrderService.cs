namespace betacomio.Services.OrderService
{
    // Implementa l'interfaccia IOrderService per fornire i servizi relativi agli ordini
    public class OrderService : IOrderService
    {
        // Lista fittizia di ordini (utilizzata solo a scopo di esempio, potrebbe essere rimossa)
        private static List<Order> orders = new List<Order>
        {
            new Order(),
            new Order { Id = 1, ProductName = "Prodotto" }
        };

        // Dichiarazione delle dipendenze utilizzate all'interno della classe
        private readonly IMapper _mapper; // Oggetto AutoMapper per la mappatura degli oggetti
        private readonly DataContext _context; // Oggetto per accedere al contesto del database DataContext
        private readonly IHttpContextAccessor _httpContextAccessor; // Oggetto per accedere al contesto HTTP
        private readonly DataContext2 _context2; // Oggetto per accedere al contesto del database DataContext2

        private static Logger logger= LogManager.GetCurrentClassLogger();
        // Costruttore della classe, viene utilizzato per iniettare le dipendenze necessarie
        public OrderService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor, DataContext2 context2)
        {
            _context2 = context2; // Inizializza l'oggetto per accedere al contesto del database DataContext2
            _httpContextAccessor = httpContextAccessor; // Inizializza l'oggetto per accedere al contesto HTTP
            _mapper = mapper; // Inizializza l'oggetto AutoMapper
            _context = context; // Inizializza l'oggetto per accedere al contesto del database DataContext
        }

        // Metodo privato per ottenere l'ID dell'utente dal contesto HTTP utilizzando i claim di sicurezza
        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User
            .FindFirstValue(ClaimTypes.NameIdentifier)!);

        // Metodo per aggiungere un nuovo ordine e restituire la lista aggiornata di GetOrderDto
        public async Task<ServiceResponse<List<GetOrderDto>>> AddOrder(AddOrderDto newOrder)
        {
            // Creazione dell'oggetto di risposta del servizio
            var serviceResponse = new ServiceResponse<List<GetOrderDto>>();
            
            try
            {
            // Mapping dell'oggetto AddOrderDto a un oggetto Order utilizzando AutoMapper
            var order = _mapper.Map<Order>(newOrder);

            // Imposta l'utente associato all'ordine ottenendo l'utente corrente dal DataContext
            order.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());

            // Aggiunge l'ordine al DataContext e salva le modifiche nel database
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Ottiene tutti gli ordini dell'utente dal DataContext utilizzando LINQ e proietta i risultati a GetOrderDto
            serviceResponse.Data = await _context.Orders
                .Where(c => c.User!.Id == GetUserId())
                .Select(c => _mapper.Map<GetOrderDto>(c))
                .ToListAsync();
            }
            catch (Exception ex)
            {
                // Se si verifica un'eccezione durante l'aggiornamento dell'ordine, imposta il flag Success su false e aggiunge il messaggio di errore al campo Message dell'oggetto di risposta
                serviceResponse.Success = false;
                serviceResponse.Message = ex.InnerException.Message;
                logger.Trace(ex.InnerException.Message, ex.Message);

            }


            // Restituzione dell'oggetto di risposta contenente la lista di GetOrderDto aggiornata
            return serviceResponse;
        }

        // Metodo per eliminare un ordine per ID e restituire la lista aggiornata di GetOrderDto
        public async Task<ServiceResponse<List<GetOrderDto>>> DeleteOrder(int id)
        {
            // Creazione dell'oggetto di risposta del servizio
            var serviceResponse = new ServiceResponse<List<GetOrderDto>>();

            try
            {
                // Ottiene l'ordine dal DataContext in base all'ID e all'ID dell'utente utilizzando LINQ
                var order = await _context.Orders
                    .FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == GetUserId());

                // Se l'ordine non viene trovato o non appartiene all'utente corrente, solleva un'eccezione
                if (order is null)
                    throw new Exception($"Order with ID {id} not found or does not belong to the current user");

                // Rimuove l'ordine dal DataContext e salva le modifiche nel database
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();

                // Ottiene tutti gli ordini dell'utente dal DataContext utilizzando LINQ e proietta i risultati a GetOrderDto
                serviceResponse.Data = await _context.Orders
                    .Where(c => c.User!.Id == GetUserId())
                    .Select(c => _mapper.Map<GetOrderDto>(c))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Se si verifica un'eccezione durante l'eliminazione dell'ordine, imposta il flag Success su false e aggiunge il messaggio di errore al campo Message dell'oggetto di risposta
                serviceResponse.Success = false;
                serviceResponse.Message = ex.InnerException.Message;
                
                logger.Trace(ex.InnerException.Message, ex.Message);
            }

            // Restituzione dell'oggetto di risposta contenente la lista di GetOrderDto aggiornata o l'eventuale messaggio di errore
            return serviceResponse;
        }

        // Metodo per ottenere tutti gli ordini e restituire una lista di GetOrderDto
        public async Task<ServiceResponse<List<GetOrderDto>>> GetAllOrder()
        {
            
            // Creazione dell'oggetto di risposta del servizio
            var serviceResponse = new ServiceResponse<List<GetOrderDto>>();
            try
            {
            var dbOrders = await _context.Orders.Where(c => c.User!.Id == GetUserId()).OrderByDescending(x => x.DateOrder).ToListAsync();
            serviceResponse.Data = dbOrders.Select(c => _mapper.Map<GetOrderDto>(c)).ToList();
            }
            catch (Exception ex)
            {
                // Se si verifica un'eccezione durante l'aggiornamento dell'ordine, imposta il flag Success su false e aggiunge il messaggio di errore al campo Message dell'oggetto di risposta
                serviceResponse.Success = false;
                serviceResponse.Message = ex.InnerException.Message;
                logger.Trace(ex.InnerException.Message, ex.Message);

            }
            // Ottiene tutti gli ordini dell'utente dal DataContext utilizzando LINQ e proietta i risultati a GetOrderDto


            // Restituzione dell'oggetto di risposta contenente la lista di GetOrderDto
            return serviceResponse;
        }

        // Metodo per ottenere un ordine per ID e restituire l'oggetto GetOrderDto corrispondente
        public async Task<ServiceResponse<GetOrderDto>> GetOrderById(int id)
        {
            // Creazione dell'oggetto di risposta del servizio
            var serviceResponse = new ServiceResponse<GetOrderDto>();
            try
            {
            var dbOrder = await _context.Orders
                .FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == GetUserId());

            // Mapping dell'ordine ottenuto a un oggetto GetOrderDto utilizzando AutoMapper
            serviceResponse.Data = _mapper.Map<GetOrderDto>(dbOrder);
            }
            catch (Exception ex)
            {
                // Se si verifica un'eccezione durante l'aggiornamento dell'ordine, imposta il flag Success su false e aggiunge il messaggio di errore al campo Message dell'oggetto di risposta
                serviceResponse.Success = false;
                serviceResponse.Message = ex.InnerException.Message;
                logger.Trace(ex.InnerException.Message, ex.Message);

            }


            // Restituzione dell'oggetto di risposta contenente l'oggetto GetOrderDto
            return serviceResponse;
        }

        // Metodo per aggiornare un ordine esistente e restituire l'oggetto GetOrderDto aggiornato
        public async Task<ServiceResponse<GetOrderDto>> UpdateOrder(UpdateOrderDto updatedOrder)
        {
            // Creazione dell'oggetto di risposta del servizio
            var serviceResponse = new ServiceResponse<GetOrderDto>();

            try
            {
                // Ottiene l'ordine dal DataContext in base all'ID utilizzando LINQ e include anche l'utente associato all'ordine
                var order = await _context.Orders
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.Id == updatedOrder.Id);

                // Se l'ordine non viene trovato o non appartiene all'utente corrente, solleva un'eccezione
                if (order is null || order.User!.Id != GetUserId())
                    throw new Exception($"Order with ID {updatedOrder.Id} not found or does not belong to the current user");

                // Aggiorna i dati dell'ordine con i valori forniti nel DTO
                order.ProductName = updatedOrder.ProductName;
                order.Quantity = updatedOrder.Quantity;
                order.Price = updatedOrder.Price;
                order.Phone = updatedOrder.Phone;

                // Salva le modifiche nel database
                await _context.SaveChangesAsync();

                // Mapping dell'ordine aggiornato a un oggetto GetOrderDto utilizzando AutoMapper
                serviceResponse.Data = _mapper.Map<GetOrderDto>(order);
            }
            catch (Exception ex)
            {
                // Se si verifica un'eccezione durante l'aggiornamento dell'ordine, imposta il flag Success su false e aggiunge il messaggio di errore al campo Message dell'oggetto di risposta
                serviceResponse.Success = false;
                serviceResponse.Message = ex.InnerException.Message;
                
                logger.Trace(ex.InnerException.Message, ex.Message);
            }

            // Restituzione dell'oggetto di risposta contenente l'oggetto GetOrderDto aggiornato o l'eventuale messaggio di errore
            return serviceResponse;
        }
    
    }
}

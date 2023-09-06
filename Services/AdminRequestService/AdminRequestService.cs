namespace betacomio.Services.AdminRequestService
{
    public class AdminRequestService : IAdminRequestService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        private static Logger logger = LogManager.GetCurrentClassLogger();
        public AdminRequestService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ServiceResponse<List<AdminRequest>>> GetAllReq()
        {
            // Creazione dell'oggetto di risposta del servizio
            var serviceResponse = new ServiceResponse<List<AdminRequest>>();

            try
            {
                // Ottenimento di tutti i prodotti dal database utilizzando Entity Framework Core (ToListAsync)
                var dbReq = await _context.AdminRequest
                    .Include(h => h.User)
                    .Where(e => e.IsAccepted == null)
                    .ToListAsync();

                // Mapping dei prodotti del database a oggetti GetProductDto utilizzando AutoMapper (Select e Map)
                serviceResponse.Data = dbReq
                .Select(req => req).ToList();
            }
            catch (Exception ex)
            {
                // Se si verifica un'eccezione durante l'aggiornamento dell'ordine, imposta il flag Success su false e aggiunge il messaggio di errore al campo Message dell'oggetto di risposta
                serviceResponse.Success = false;
                serviceResponse.Message = ex.InnerException.Message;
                logger.Trace(ex.InnerException.Message, ex.Message);

            }



            // Restituzione dell'oggetto di risposta contenente la lista di GetProductDto
            return serviceResponse;

        }

        public async Task<ServiceResponse<int>> GetAllReqCount()
        {
            var serviceResponse = new ServiceResponse<int>();
            try
            {
                var dbReqCount = await _context.AdminRequest
                                .Include(h => h.User)
                                .Where(e => e.IsAccepted == null)
                                .CountAsync();

                serviceResponse.Data = dbReqCount;
            }
            catch (Exception ex)
            {
                // Se si verifica un'eccezione durante l'aggiornamento dell'ordine, imposta il flag Success su false e aggiunge il messaggio di errore al campo Message dell'oggetto di risposta
                serviceResponse.Success = false;
                serviceResponse.Message = ex.InnerException.Message;
                logger.Trace(ex.InnerException.Message, ex.Message);

            }


            return serviceResponse;

        }
        public async Task<ServiceResponse<PutReqDto>> UpdateReq(int id, PutReqDto putDto)
        {

            // Creazione dell'oggetto di risposta del servizio
            var serviceResponse = new ServiceResponse<PutReqDto>();
            if (id != putDto.IdRequest)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "errore";

                logger.Trace(serviceResponse.Message);
                return serviceResponse;

            }
            try
            {
                // Ottiene la richiesta dal DataContext in base all'ID utilizzando LINQ e include anche l'utente associato all'ordine
                var req = await _context.AdminRequest
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.User.Id == putDto.UserId);

                // Aggiorna i dati dell'ordine con i valori forniti nel DTO
                req.IdRequest = putDto.IdRequest;
                req.UserId = putDto.UserId;
                req.IsAccepted = putDto.IsAccepted;

                // Salva le modifiche nel database
                await _context.SaveChangesAsync();

                // Mapping dell'ordine aggiornato a un oggetto GetOrderDto utilizzando AutoMapper
                serviceResponse.Data = _mapper.Map<PutReqDto>(req);
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
        public async Task<ServiceResponse<AdminRequest>> CreateAdminRequest(AdminRequest adminRequest)
        {
            var serviceResponse = new ServiceResponse<AdminRequest>();

            //Calcolo del numero di richieste fatte dall'utente loggato
            var numReq = await _context.AdminRequest.Where(c => c.User!.Id == adminRequest.UserId).CountAsync();

            try
            {
                //controllo per verificare se l'utente ha fatto piu di una richiesta
                if (numReq > 0)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Hai fatto troppe richieste";

                    logger.Trace(serviceResponse.Message);

                }
                else
                {
                    // Aggiungi l'AdminRequest al DataContext
                    _context.AdminRequest.Add(adminRequest);
                    // Salva le modifiche nel database
                    await _context.SaveChangesAsync();

                    // Mapping dell'AdminRequest aggiunto a un oggetto GetAdminRequestDto utilizzando AutoMapper
                    serviceResponse.Data = _mapper.Map<AdminRequest>(adminRequest);
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.InnerException.Message;

                logger.Trace(serviceResponse.Message);
            }

            return serviceResponse;
        }
        // AdminRequestService.cs
        public async Task<ServiceResponse<string>> DeleteUserRequest(int userId)
        {
            var serviceResponse = new ServiceResponse<string>();

            try
            {
                // Trova la prima richiesta in sospeso dell'utente loggato
                var requestToDelete = await _context.AdminRequest
                    .FirstOrDefaultAsync(r => r.UserId == userId && r.IsAccepted == null);

                if (requestToDelete == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Non hai richieste da eliminare.";

                    logger.Trace(serviceResponse.Message);
                    return serviceResponse;
                }

                _context.AdminRequest.Remove(requestToDelete);
                await _context.SaveChangesAsync();

                serviceResponse.Data = "Richiesta eliminata con successo.";
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.InnerException.Message;

                logger.Trace(serviceResponse.Message);
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<int>> GetUserRequestCount(int userId)
        {
            var serviceResponse = new ServiceResponse<int>();

            try
            {
                var userRequestCount = await _context.AdminRequest
                                .Where(r => r.UserId == userId)
                                .CountAsync();

                serviceResponse.Data = userRequestCount;
            }
            catch (Exception ex)
            {
                // Se si verifica un'eccezione durante l'aggiornamento dell'ordine, imposta il flag Success su false e aggiunge il messaggio di errore al campo Message dell'oggetto di risposta
                serviceResponse.Success = false;
                serviceResponse.Message = ex.InnerException.Message;
                logger.Trace(ex.InnerException.Message, ex.Message);

            }


            return serviceResponse;
        }
    }
}

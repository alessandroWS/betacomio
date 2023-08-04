using betacomio.Dtos.AdminRequest;
using Microsoft.AspNetCore.Http.HttpResults;

namespace betacomio.Services.AdminRequestService
{
    public class AdminRequestService : IAdminRequestService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public AdminRequestService (IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ServiceResponse<List<AdminRequest>>> GetAllReq()
        {
            // Creazione dell'oggetto di risposta del servizio
            var serviceResponse = new ServiceResponse<List<AdminRequest>>();

            // Ottenimento di tutti i prodotti dal database utilizzando Entity Framework Core (ToListAsync)
            var dbReq = await _context.AdminRequest
                .Include(h => h.User)
                .Where(e => e.IsAccepted == null)
                .ToListAsync();

            // Mapping dei prodotti del database a oggetti GetProductDto utilizzando AutoMapper (Select e Map)
            serviceResponse.Data = dbReq
            .Select(req => req).ToList();

            // Restituzione dell'oggetto di risposta contenente la lista di GetProductDto
            return serviceResponse;

        }
         public async Task<ServiceResponse<PutReqDto>> UpdateReq(int id, PutReqDto putDto)
        {
            // Creazione dell'oggetto di risposta del servizio
            var serviceResponse = new ServiceResponse<PutReqDto>();
            if (id != putDto.IdRequest)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "esplodi porcodio";
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
            }

            // Restituzione dell'oggetto di risposta contenente l'oggetto GetOrderDto aggiornato o l'eventuale messaggio di errore
            return serviceResponse;
        }
    
    }
}

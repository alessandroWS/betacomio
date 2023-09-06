namespace betacomio.Services.LikesService
{
    public class LikesService : ILikesService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private static Logger logger= LogManager.GetCurrentClassLogger();
        public LikesService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _context = context;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User
            .FindFirstValue(ClaimTypes.NameIdentifier)!);

        public async Task<ServiceResponse<AddLikesDto>> AddLikes(AddLikesDto addlikes)
        {
            var serviceResponse = new ServiceResponse<AddLikesDto>();

            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
                if (user != null)
                {
                    var existingLike = await _context.Like
                        .FirstOrDefaultAsync(l => l.ProductId == addlikes.ProductId && l.User!.Id == GetUserId());

                    if (existingLike != null)
                    {
                        serviceResponse.Success = false;
                        serviceResponse.Message = "Il prodotto è già presente tra i preferiti.";
                        
                logger.Trace(serviceResponse.Message);
                    }
                    else
                    {
                        var like = _mapper.Map<Like>(addlikes);
                        like.User = user;

                        _context.Like.Add(like);
                        await _context.SaveChangesAsync();

                        serviceResponse.Data = _mapper.Map<AddLikesDto>(like);
                        serviceResponse.Message = "Prodotto inserito correttamente tra i preferiti.";
                    }
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Utente non trovato.";
                    
                logger.Trace(serviceResponse.Message);
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                
                logger.Trace(ex.InnerException, ex.Message);
            }

            return serviceResponse;
        }


        public async Task<ServiceResponse<List<AddLikesDto>>> GetAllLikes(int userId)
        {
            var serviceResponse = new ServiceResponse<List<AddLikesDto>>();

            try
            {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user != null)
            {
                var likes = await _context.Like.Where(c => c.User!.Id == userId).ToListAsync();
                serviceResponse.Data = likes.Select(c => _mapper.Map<AddLikesDto>(c)).ToList();
            }
            else
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Utente non trovato.";
                
                logger.Trace(serviceResponse.Message);
            }
            }
            catch (Exception ex)
            {
                // Se si verifica un'eccezione durante l'aggiornamento dell'ordine, imposta il flag Success su false e aggiunge il messaggio di errore al campo Message dell'oggetto di risposta
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                logger.Trace(ex.InnerException, ex.Message);

            }


            return serviceResponse;
        }

        public async Task<ServiceResponse<List<AddLikesDto>>> DeleteLike(int id)
        {
            // Creazione dell'oggetto di risposta del servizio
            var serviceResponse = new ServiceResponse<List<AddLikesDto>>();

            try
            {
                // Ottiene l'ordine dal DataContext in base all'ID e all'ID dell'utente utilizzando LINQ
                var like = await _context.Like
                    .FirstOrDefaultAsync(c => c.ProductId == id && c.User!.Id == GetUserId());

                // Se l'ordine non viene trovato o non appartiene all'utente corrente, solleva un'eccezione
                if (like is null)
                    throw new Exception($"Order with ID {id} not found or does not belong to the current user");

                // Rimuove l'ordine dal DataContext e salva le modifiche nel database
                _context.Like.Remove(like);
                await _context.SaveChangesAsync();

                // Ottiene tutti gli ordini dell'utente dal DataContext utilizzando LINQ e proietta i risultati a GetOrderDto
                serviceResponse.Data = await _context.Like
                    .Where(c => c.User!.Id == GetUserId())
                    .Select(c => _mapper.Map<AddLikesDto>(c))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Se si verifica un'eccezione durante l'eliminazione dell'ordine, imposta il flag Success su false e aggiunge il messaggio di errore al campo Message dell'oggetto di risposta
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                
                logger.Trace(ex.InnerException, ex.Message);
            }

            // Restituzione dell'oggetto di risposta contenente la lista di GetOrderDto aggiornata o l'eventuale messaggio di errore
            return serviceResponse;
        }
    }
}

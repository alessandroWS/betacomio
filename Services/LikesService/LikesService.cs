using betacomio.Dtos.Likes;

namespace betacomio.Services.LikesService
{
    public class LikesService : ILikesService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor; // Oggetto per accedere al contesto HTTP

        public LikesService (IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor; // Inizializza l'oggetto per accedere al contesto HTTP
            _mapper = mapper; // Inizializza l'oggetto AutoMapper
            _context = context; // Inizializza l'oggetto per accedere al contesto del database DataContext
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User
        .FindFirstValue(ClaimTypes.NameIdentifier)!);

        public async Task<ServiceResponse<List<AddLikesDto>>> AddLikes(AddLikesDto addlike)
        {
            var serviceResponse = new ServiceResponse<List<AddLikesDto>>();

            try{
            // Creazione dell'oggetto di risposta del servizio

            // Mapping dell'oggetto AddOrderDto a un oggetto Order utilizzando AutoMapper
            var like = _mapper.Map<Like>(addlike);

            // Imposta l'utente associato all'ordine ottenendo l'utente corrente dal DataContext
            like.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());

            // Aggiunge l'ordine al DataContext e salva le modifiche nel database
            _context.Like.Add(like);
            await _context.SaveChangesAsync();

             serviceResponse.Data = await _context.Like
                .Where(c => c.User!.Id == GetUserId())
                .Select(c => _mapper.Map<AddLikesDto>(c))
                .ToListAsync();
  
            serviceResponse.Message = "Inserito correttamente fra i preferiti";

            // Restituzione dell'oggetto di risposta contenente la lista di GetOrderDto aggiornata
            return serviceResponse;

            } catch (Exception ex){
                // Se si verifica un'eccezione durante l'eliminazione dell'ordine, imposta il flag Success su false e aggiunge il messaggio di errore al campo Message dell'oggetto di risposta
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                return serviceResponse;

            }
        }

        public async Task<ServiceResponse<List<AddLikesDto>>> GetAllLikes(int userId)
        {
             // Creazione dell'oggetto di risposta del servizio
            var serviceResponse = new ServiceResponse<List<AddLikesDto>>();

            // Ottiene tutti gli ordini dell'utente dal DataContext utilizzando LINQ e proietta i risultati a GetOrderDto
            var awe = await _context.Like.Where(c => c.User!.Id == GetUserId()).ToListAsync();
            foreach (var like in awe)
                {
                    like.User!.Id = GetUserId();
                }
            serviceResponse.Data = awe.Select(c => _mapper.Map<AddLikesDto>(c)).ToList();
            

            // Restituzione dell'oggetto di risposta contenente la lista di GetOrderDto
            return serviceResponse;
        }
    }
}

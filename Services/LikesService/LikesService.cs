using betacomio.Dtos.Likes;

namespace betacomio.Services.LikesService
{
    public class LikesService : ILikesService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LikesService (IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _context = context;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User
            .FindFirstValue(ClaimTypes.NameIdentifier)!);

        public async Task<ServiceResponse<List<AddLikesDto>>> AddLikes(AddLikesDto addlike)
        {
            var serviceResponse = new ServiceResponse<List<AddLikesDto>>();

            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
                if (user != null)
                {
                    var like = _mapper.Map<Like>(addlike);
                    like.User = user;

                    _context.Like.Add(like);
                    await _context.SaveChangesAsync();

                    serviceResponse.Data = await _context.Like
                        .Where(c => c.User!.Id == GetUserId())
                        .Select(c => _mapper.Map<AddLikesDto>(c))
                        .ToListAsync();

                    serviceResponse.Message = "Inserito correttamente fra i preferiti";
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Utente non trovato.";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<AddLikesDto>>> GetAllLikes(int userId)
        {
            var serviceResponse = new ServiceResponse<List<AddLikesDto>>();

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
            }

            return serviceResponse;
        }
    }
}

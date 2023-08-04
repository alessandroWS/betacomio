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

        public async Task<ServiceResponse<List<AdminRequestInfoDto>>> GetAllRequestAdmin()
        {
            var serviceResponse = new ServiceResponse<List<AdminRequestInfoDto>>();
            var dbProduct= await _context.AdminRequests.ToListAsync();
            serviceResponse.Data = dbProduct.Select(c => _mapper.Map<AdminRequestInfoDto>(c)).ToList();
            return serviceResponse;
        }

    }

}

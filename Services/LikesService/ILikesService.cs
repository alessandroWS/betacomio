namespace betacomio.Services.LikesService
{
    public interface ILikesService
    {
        Task<ServiceResponse<AddLikesDto>> AddLikes(AddLikesDto addlikes);
        Task<ServiceResponse<List<AddLikesDto>>> GetAllLikes(int userId);
        
       Task<ServiceResponse<List<AddLikesDto>>> DeleteLike(int id);
    }
}
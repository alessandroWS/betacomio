using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using betacomio.Dtos.Likes;

namespace betacomio.Services.LikesService
{
    public interface ILikesService
    {
        Task<ServiceResponse<AddLikesDto>> AddLikes(AddLikesDto addlikes);
        Task<ServiceResponse<List<AddLikesDto>>> GetAllLikes(int userId);
        
       Task<ServiceResponse<List<AddLikesDto>>> DeleteLike(int id);
    }
}
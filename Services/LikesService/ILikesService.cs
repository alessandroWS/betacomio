using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using betacomio.Dtos.Likes;

namespace betacomio.Services.LikesService
{
    public interface ILikesService
    {
        // Task<ServiceResponse<List<AddLikesDto>>> GetAllOrder();

        // Metodo per ottenere un ordine per ID e restituire l'oggetto GetOrderDto corrispondente

        // Metodo per aggiungere un nuovo ordine e restituire la lista aggiornata di GetOrderDto
        Task<ServiceResponse<List<AddLikesDto>>> AddLikes(AddLikesDto addlikes);
        Task<ServiceResponse<List<AddLikesDto>>> GetAllLikes(int userId);
        
    }
}
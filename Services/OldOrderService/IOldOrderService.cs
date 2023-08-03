global using betacomio.Dtos.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace betacomio.Services.OldOrderService
{
    public interface IOldOrderService
    {
        Task<ServiceResponse<List<OldOrderDto>>> GetAllOldOrderForCustomer();
    }
}
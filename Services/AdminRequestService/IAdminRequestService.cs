using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace betacomio.Services.AdminRequestService
{
    public interface IAdminRequestService
    {
        Task<ServiceResponse<List<AdminRequestInfoDto>>> GetAllRequestAdmin();
    }
}
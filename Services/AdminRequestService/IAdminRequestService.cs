using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using betacomio.Dtos.AdminRequest;

namespace betacomio.Services.AdminRequestService
{
    public interface IAdminRequestService
    {
        Task<ServiceResponse<List<AdminRequest>>> GetAllReq();
       Task<ServiceResponse<PutReqDto>> UpdateReq(int id, PutReqDto putDto);
       Task<ServiceResponse<AdminRequest>> CreateAdminRequest(AdminRequest adminRequest);
    }
}
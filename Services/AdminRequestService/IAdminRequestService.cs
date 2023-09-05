namespace betacomio.Services.AdminRequestService
{
    public interface IAdminRequestService
    {
        Task<ServiceResponse<List<AdminRequest>>> GetAllReq();
        Task<ServiceResponse<int>> GetAllReqCount();
        Task<ServiceResponse<PutReqDto>> UpdateReq(int id, PutReqDto putDto);
        Task<ServiceResponse<AdminRequest>> CreateAdminRequest(AdminRequest adminRequest);
        
        // Nuovi metodi per l'eliminazione e il conteggio delle richieste dell'utente loggato
        Task<ServiceResponse<string>> DeleteUserRequest(int userId);
        Task<ServiceResponse<int>> GetUserRequestCount(int userId);
    }
}

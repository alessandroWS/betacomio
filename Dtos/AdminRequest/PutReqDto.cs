namespace betacomio.Dtos.AdminRequest
{
    public class PutReqDto
    {
        public int IdRequest {get; set;}
        
        public bool? IsAccepted {get; set;} = null;
        public int UserId {get; set;}
    }
}
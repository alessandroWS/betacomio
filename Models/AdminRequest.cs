namespace betacomio.Models
{
    public class AdminRequest
    {
        public int IdRequest {get; set;}
        public bool? IsAccepted {get; set;}
        public int IdUser {get; set;}
        public DateTime Date {get; set;}
    }
}
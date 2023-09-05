namespace betacomio.Models
{
    public class AdminRequest
    {
        public int IdRequest {get; set;}
        public bool? IsAccepted {get; set;} = null;
        public int UserId {get; set;}
        public DateTime? Date {get; set;} = DateTime.Now;
        public User? User { get; set; }
    }
}
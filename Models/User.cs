namespace betacomio.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public bool? IsAdmin { get; set; } = false;
        public List<Order>? orders{ get; set; } 
        public List<Like>? likes{ get; set; } 


    }
}
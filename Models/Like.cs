
namespace betacomio.Models
{
    public class Like
    {
        [Key]
        public int IdLike {get; set;}
        public string ProductName {get; set;} 
        public int ProductId {get; set;} 
        public string Price {get; set;}
        public string CategoryName {get; set;}

        public User? User { get; set; }

    }
}
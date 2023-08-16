using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace betacomio.Models
{
    public class Like
    {
        [Key]
        public int IdLike {get; set;}
        public string ProductName {get; set;} 
        public int Price {get; set;}

        public User? User { get; set; }

    }
}
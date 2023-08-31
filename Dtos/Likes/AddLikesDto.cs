namespace betacomio.Dtos.Likes
{
    public class AddLikesDto
    {
       
        public string ProductName {get; set;} 
        public string Price {get; set;}
        public int ProductId { get; set; }
        public string CategoryName {get; set;}

        public int UserId {get; set;}


    }
}
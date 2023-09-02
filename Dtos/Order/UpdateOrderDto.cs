namespace betacomio.Services.OrderService
{
    public class UpdateOrderDto
    {
        public int Id {get; set;}
        public string ProductName {get; set;} = "";

        public int Quantity {get; set;}
        public string Price {get; set;}
        public DateTime DateOrder {get; set;} = DateTime.Now;
        public string Phone {get; set;}
    }
}
namespace betacomio.Dtos.Order
{
    public class GetOrderDto
    {
        public int Id {get; set;}
        public string ProductName {get; set;} = "";

        public int Quantity {get; set;}
        public int Price {get; set;}
        public DateTime DateOrder {get; set;} = DateTime.Now;
        public int Phone {get; set;}

    }
}
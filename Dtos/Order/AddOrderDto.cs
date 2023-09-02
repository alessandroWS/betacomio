namespace betacomio.Dtos.Order
{
    public class AddOrderDto
    {
        public string ProductName {get; set;} = "";

        public int Quantity {get; set;}
        public string Price {get; set;}
        public string Phone {get; set;}

    }
}
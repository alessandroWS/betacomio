namespace betacomio.Dtos.Order
{
    public class AddOrderDto
    {
        public string ProductName {get; set;} = "";

        public int Quantity {get; set;}
        public string Price {get; set;}
        public string Phone {get; set;}
        public string Address {get; set;}
        public int CAP {get; set;}
        public string City {get; set;}
        public string FirstName {get; set;}
        public string Surname {get; set;}

    }
}
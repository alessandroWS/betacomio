using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace betacomio.Dtos.Order
{
    public class AddOrderDto
    {
        public string ProductName {get; set;} = "";

        public int Quantity {get; set;}
        public int Price {get; set;}
        public int Phone {get; set;}

    }
}
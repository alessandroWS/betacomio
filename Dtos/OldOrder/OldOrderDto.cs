using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace betacomio.Dtos.OldOrder
{
    public class OldOrderDto
    {
        public int CustomerID { get; set; }
        public string EmailAddress { get; set; }
        public int SalesOrderID { get; set; }
        public int SalesOrderDetailID { get; set; }
        public DateTime OrderDate { get; set; }
        public int OrderQty { get; set; }
        public int ProductID { get; set; }
        public string Name { get; set; }
    }
}
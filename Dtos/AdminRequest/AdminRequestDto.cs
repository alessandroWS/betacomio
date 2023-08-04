using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace betacomio.Dtos.OldOrder
{
    public class AdminRequestInfoDto
    {
        public int CustomerID { get; set; }
        public string EmailAddress { get; set; }
        public DateTime Date { get; set; }
    }
}
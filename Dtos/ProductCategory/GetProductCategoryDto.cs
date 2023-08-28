using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace betacomio.Dtos.ProductCategory
{
    public class GetProductCategoryDto
    {
        public int ProductCategoryId { get; set; }
        public string Name { get; set; } = null!;

        public string? Img { get; set; }
    }
}
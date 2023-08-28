using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using betacomio.Dtos.ProductCategory;

namespace betacomio.Services.ProductCategoryService
{
    public interface IProductCategoryService
    {
        Task<ServiceResponse<List<GetProductCategoryDto>>> GetProductCategory();

        
    }
}
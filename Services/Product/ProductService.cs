global using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using betacomio.Models;

namespace betacomio.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;
        private readonly AdventureWorksLt2019Context _adventure;

        public ProductService (IMapper mapper, IHttpContextAccessor httpContext, AdventureWorksLt2019Context adventure)
        {
            _mapper = mapper;
            _httpContext = httpContext;
            _adventure = adventure;
        }

        public Task<ServiceResponse<List<GetProductDto>>> AddProduct(AddProductDto newProduct)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<List<GetProductDto>>> DeleteProduct(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<List<GetProductDto>>> GetAllProduct()
        {
            var serviceResponse = new ServiceResponse<List<GetProductDto>>();
            var dbProduct= await _adventure.Products.ToListAsync();
            serviceResponse.Data = dbProduct.Select(c => _mapper.Map<GetProductDto>(c)).ToList();
            return serviceResponse;
        }

        public Task<ServiceResponse<GetProductDto>> GetProductById(int id)
        {
            throw new NotImplementedException();
        }
    }
    
}
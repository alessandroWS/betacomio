namespace betacomio.Services.ProductServices
{
    public interface IProductService
    {
        Task<ServiceResponse<List<Product>>> GetAllProduct();
        Task<ServiceResponse<Product>> GetProductById(int id);
        Task<ServiceResponse<List<Product>>> GetProductByIdCategory(int id);
        Task<ServiceResponse<Product>> AddProducts(AddProductDto newProduct);
    }
}

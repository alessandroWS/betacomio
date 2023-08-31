namespace betacomio.Services.ProductServices
{
    public interface IProductService
    {
        // Ottiene tutti i prodotti e restituisce una lista di GetProductDto (DTO = Data Transfer Object)
        Task<ServiceResponse<List<Product>>> GetAllProduct();
        Task<ServiceResponse<Product>> GetProductById(int id);
        Task<ServiceResponse<List<Product>>> GetProductByIdCategory(int id);

       
    }
}

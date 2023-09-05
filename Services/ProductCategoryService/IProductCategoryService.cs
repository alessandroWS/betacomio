namespace betacomio.Services.ProductCategoryService
{
    public interface IProductCategoryService
    {
        Task<ServiceResponse<List<GetProductCategoryDto>>> GetProductCategory();

        Task<ServiceResponse<ProductCategory>> GetProductCategoryById(int id);

    }
}
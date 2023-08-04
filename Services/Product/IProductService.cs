namespace betacomio.Services.ProductServices
{
    public interface IProductService
    {
        // Ottiene tutti i prodotti e restituisce una lista di GetProductDto (DTO = Data Transfer Object)
        Task<ServiceResponse<List<GetProductDto>>> GetAllProduct();

        // Ottiene un prodotto per ID e restituisce l'oggetto GetProductDto corrispondente
        Task<ServiceResponse<GetProductDto>> GetProductById(int id);

        // Aggiunge un nuovo prodotto e restituisce la lista aggiornata di GetProductDto
        Task<ServiceResponse<List<GetProductDto>>> AddProduct(AddProductDto newProduct);

        // Elimina un prodotto per ID e restituisce la lista aggiornata di GetProductDto
        Task<ServiceResponse<List<GetProductDto>>> DeleteProduct(int id);
    }
}

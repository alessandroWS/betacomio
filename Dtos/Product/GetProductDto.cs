namespace betacomio.Dtos.Product
{
    public class GetProductDto
    {

    public string Name { get; set; } = null!;

    public string ProductNumber { get; set; } = null!;

    public decimal StandardCost { get; set; }

    public decimal ListPrice { get; set; }


    }
}
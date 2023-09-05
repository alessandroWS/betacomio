using System.ComponentModel.DataAnnotations;

public class AddProductDto
{
    [Required(ErrorMessage = "Il campo 'Name' è obbligatorio.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Il campo 'ProductNumber' è obbligatorio.")]
    public string ProductNumber { get; set; }

    [Required(ErrorMessage = "Il campo 'StandardCost' è obbligatorio.")]
    public decimal StandardCost { get; set; }

    [Required(ErrorMessage = "Il campo 'ListPrice' è obbligatorio.")]
    public decimal ListPrice { get; set; }

    [Required(ErrorMessage = "Il campo 'ProductCategoryId' è obbligatorio.")]
    public int? ProductCategoryId { get; set; } // Aggiungi ProductCategoryId come campo obbligatorio
}

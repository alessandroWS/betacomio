public partial class ProductCategory
{
    public int ProductCategoryId { get; set; }
    public int? ParentProductCategoryId { get; set; }
    public string Name { get; set; } = null!;
    public string? Img { get; set; }
    public Guid Rowguid { get; set; }
    public DateTime ModifiedDate { get; set; } = DateTime.UtcNow; // Imposta la data e l'ora correnti in UTC come valore predefinito
    public virtual ICollection<ProductCategory> InverseParentProductCategory { get; set; } = new List<ProductCategory>();
    public virtual ProductCategory? ParentProductCategory { get; set; }
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
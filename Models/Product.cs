namespace GiyimStokMvc.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ModelCode { get; set; }
    public string? Category { get; set; }
    public ICollection <ProductVariant> ProductVariants { get; set; }
}
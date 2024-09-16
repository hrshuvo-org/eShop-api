using Framework.Core.Models.Entities;

namespace Framework.App.Models.Entities;

public class ProductItem : BaseEntity<long>
{
    public long ProductId { get; set; }
    public string Product { get; set; }

    public string Sku { get; set; }

    public int QtyStock { get; set; }
    public decimal Price { get; set; }

    public string PhotoUrl { get; set; }
}
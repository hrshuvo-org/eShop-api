using Framework.Core.Models.Entities;

namespace Framework.App.Models.Entities;

public class Product : BaseEntity<long>
{
    public long CategoryId { get; set; }
    public string Category { get; set; }
    
    public long BrandId { get; set; }
    public string Brand { get; set; }
    
    
    public string Description { get; set; }

    public float Rating { get; set; }

    public int DiscountType { get; set; } // 0 - no discount, 1 - percentage, 2 - flat discount
    public int DiscountPercentage { get; set; }
    public decimal DiscountFlat { get; set; }
    public string DiscountInfo { get; set; }

    public decimal Price { get; set; }
    public decimal PriceAfterDiscount { get; set; }

    public int AvailableStatus { get; set; } // in stock, out of stock, upcoming
    public int Available { get; set; } // Available Quantity

    public string PhotoUrl { get; set; }

    public  string Specification { get; set; } // JSON data



    public List<Review> ReviewList { get; set; } = [];
}
using Framework.App.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Framework.App.Data;

public abstract class AppModelBuilder
{
    private const string TablePrefix = "shop";
    
    public static void Build(ModelBuilder builder)
    {
        #region Catalog

        builder.Entity<Category>(b =>
        {
            b.ToTable(SetTableName("categories"));
            b.HasKey(c => c.Id);
            
            // b.Property(e => e.ParentCategory).HasMaxLength(100);   
            
            b.Property(e => e.Name).IsRequired().HasMaxLength(100);
        });

        builder.Entity<Product>(b =>
        {
            b.ToTable(SetTableName("products"));
            b.Property(e => e.Name).IsRequired().HasMaxLength(250);
            b.Property(e => e.Category).HasMaxLength(100);   
            // b.Property(e => e.Brand).HasMaxLength(100);   
            // b.Property(e => e.DiscountInfo).HasMaxLength(100);
            b.Property(e => e.PhotoUrl).HasMaxLength(250);
            b.Property(e => e.Description).HasMaxLength(500);
            b.Property(e => e.Specification).HasMaxLength(1000);
        });

        builder.Entity<ProductItem>(b =>
        {
            b.ToTable(SetTableName("product_items"));
            b.Property(e => e.Name).IsRequired().HasMaxLength(250);
            
            b.Property(e => e.Product).HasMaxLength(250);   
            b.Property(e => e.Sku).HasMaxLength(100);
            
            b.Property(e => e.PhotoUrl).HasMaxLength(250);
        });

        builder.Entity<Review>(b =>
        {
            b.ToTable(SetTableName("product_reviews"));
        });

        builder.Entity<ReviewReply>(b =>
        {
            b.ToTable(SetTableName("product_review_replies"));
        });



        #endregion



    }

    private static string SetTableName(string tableName)
    {
        return TablePrefix + "_" + tableName;
    }
}
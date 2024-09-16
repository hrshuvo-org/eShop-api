using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class AddProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ParentCategory",
                table: "shop_categories",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ParentCategoryId",
                table: "shop_categories",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "shop_products",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false),
                    Category = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    BrandId = table.Column<long>(type: "bigint", nullable: false),
                    Brand = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    Rating = table.Column<float>(type: "float", nullable: false),
                    DiscountType = table.Column<int>(type: "int", nullable: false),
                    DiscountPercentage = table.Column<int>(type: "int", nullable: false),
                    DiscountFlat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountInfo = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PriceAfterDiscount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AvailableStatus = table.Column<int>(type: "int", nullable: false),
                    Available = table.Column<int>(type: "int", nullable: false),
                    PhotoUrl = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    Specification = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true),
                    Name = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shop_products", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "shop_products_reviews",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shop_products_reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_shop_products_reviews_shop_products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "shop_products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "shop_products_review_replies",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ReviewId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shop_products_review_replies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_shop_products_review_replies_shop_products_reviews_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "shop_products_reviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_shop_products_review_replies_ReviewId",
                table: "shop_products_review_replies",
                column: "ReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_shop_products_reviews_ProductId",
                table: "shop_products_reviews",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "shop_products_review_replies");

            migrationBuilder.DropTable(
                name: "shop_products_reviews");

            migrationBuilder.DropTable(
                name: "shop_products");

            migrationBuilder.DropColumn(
                name: "ParentCategory",
                table: "shop_categories");

            migrationBuilder.DropColumn(
                name: "ParentCategoryId",
                table: "shop_categories");
        }
    }
}

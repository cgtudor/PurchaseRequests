using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PurchaseRequests.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "_purchaseRequests",
                columns: table => new
                {
                    PurchaseRequestID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CardNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    When = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductEan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    BrandName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    TotalPrice = table.Column<double>(type: "float", nullable: false),
                    PurchaseRequestStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__purchaseRequests", x => x.PurchaseRequestID);
                });

            migrationBuilder.InsertData(
                table: "_purchaseRequests",
                columns: new[] { "PurchaseRequestID", "AccountName", "BrandId", "BrandName", "CardNumber", "Description", "Name", "Price", "ProductEan", "ProductId", "PurchaseRequestStatus", "Quantity", "TotalPrice", "When" },
                values: new object[,]
                {
                    { 1, "John Doe", 1, "Eypple", "4836725244064865", "Old and dusty.", "Offbrand IPhone", 1.0, "26601113", 1, 1, 4, 4.0, new DateTime(2021, 11, 28, 11, 46, 30, 566, DateTimeKind.Local).AddTicks(9896) },
                    { 2, "Johnny Silverhand", 2, "Gugle", "4890429190081675", "Old and nasty.", "Offbrand Google Pixel", 2.0, "14059292", 2, 1, 45, 90.0, new DateTime(2021, 11, 28, 11, 46, 30, 573, DateTimeKind.Local).AddTicks(5950) },
                    { 3, "S. Mario", 3, "Brick", "4556711787875527", "Old and sassy.", "Offbrand Nokia", 1.0, "62592994", 3, 2, 1243, 1243.0, new DateTime(2021, 11, 28, 11, 46, 30, 573, DateTimeKind.Local).AddTicks(6124) },
                    { 4, "G. Bowser", 4, "Whiteburry", "4539817512278671", "Old and gassy.", "Offbrand Blackburry", 3.0, "16361652", 4, 3, 23, 69.0, new DateTime(2021, 11, 28, 11, 46, 30, 573, DateTimeKind.Local).AddTicks(6134) },
                    { 5, "Yoshi", 5, "Bobsung", "4539919751889166", "Old and glassy.", "Offbrand Samsung", 1.0, "53035172", 5, 1, 43, 43.0, new DateTime(2021, 11, 28, 11, 46, 30, 573, DateTimeKind.Local).AddTicks(6139) }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "_purchaseRequests");
        }
    }
}

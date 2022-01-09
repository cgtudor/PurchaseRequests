using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PurchaseRequests.Migrations
{
    public partial class RemoveSeedersMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "_purchaseRequests",
                keyColumn: "PurchaseRequestID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "_purchaseRequests",
                keyColumn: "PurchaseRequestID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "_purchaseRequests",
                keyColumn: "PurchaseRequestID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "_purchaseRequests",
                keyColumn: "PurchaseRequestID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "_purchaseRequests",
                keyColumn: "PurchaseRequestID",
                keyValue: 5);

            migrationBuilder.DropColumn(
                name: "AccountName",
                table: "_purchaseRequests");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "_purchaseRequests");

            migrationBuilder.DropColumn(
                name: "BrandName",
                table: "_purchaseRequests");

            migrationBuilder.DropColumn(
                name: "CardNumber",
                table: "_purchaseRequests");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "_purchaseRequests");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "_purchaseRequests");

            migrationBuilder.DropColumn(
                name: "ProductEan",
                table: "_purchaseRequests");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "_purchaseRequests");

            migrationBuilder.DropColumn(
                name: "When",
                table: "_purchaseRequests");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountName",
                table: "_purchaseRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "BrandId",
                table: "_purchaseRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BrandName",
                table: "_purchaseRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CardNumber",
                table: "_purchaseRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "_purchaseRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "_purchaseRequests",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "ProductEan",
                table: "_purchaseRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "TotalPrice",
                table: "_purchaseRequests",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "When",
                table: "_purchaseRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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
    }
}

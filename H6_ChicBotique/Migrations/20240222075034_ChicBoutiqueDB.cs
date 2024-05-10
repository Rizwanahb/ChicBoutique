using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace H6_ChicBotique.Migrations
{
    public partial class ChicBoutiqueDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(20)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    TransactionId = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Amount = table.Column<decimal>(type: "Decimal(10,3)", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    TimePaid = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(32)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(32)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(32)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(32)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(32)", nullable: false),
                    Stock = table.Column<short>(type: "smallint", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AccountInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountInfo_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PasswordEntity",
                columns: table => new
                {
                    PasswordId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(64)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordEntity", x => x.PasswordId);
                    table.ForeignKey(
                        name: "FK_PasswordEntity_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HomeAddress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TelePhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountInfoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HomeAddress_AccountInfo_AccountInfoId",
                        column: x => x.AccountInfoId,
                        principalTable: "AccountInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderDate = table.Column<DateTime>(type: "Date", nullable: false, defaultValueSql: "getdate()"),
                    AccountInfoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_AccountInfo_AccountInfoId",
                        column: x => x.AccountInfoId,
                        principalTable: "AccountInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_Payment_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ProductTitle = table.Column<string>(type: "nvarchar(32)", nullable: false),
                    ProductPrice = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    Quantity = table.Column<short>(type: "smallint", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShippingDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShippingDetails_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "Id", "CategoryName" },
                values: new object[,]
                {
                    { 1, "Kids" },
                    { 2, "Men" },
                    { 3, "Women" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Role" },
                values: new object[,]
                {
                    { 1, "peter@abc.com", "Peter", "Aksten", 0 },
                    { 2, "riz@abc.com", "Rizwanah", "Mustafa", 1 },
                    { 3, "afr@abc.com", "Afrina", "Rahaman", 2 }
                });

            migrationBuilder.InsertData(
                table: "AccountInfo",
                columns: new[] { "Id", "UserId" },
                values: new object[,]
                {
                    { new Guid("4bb0b59f-30d3-4413-8ad3-36b4e61393cc"), 2 },
                    { new Guid("ca635ab3-d525-4e4f-ad94-69b7c6441427"), 1 }
                });

            migrationBuilder.InsertData(
                table: "PasswordEntity",
                columns: new[] { "PasswordId", "LastUpdated", "Password", "Salt", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "03BDA8F017C7923B398D7725A9920916FD93001644986F4C933DDD4481742515", "ZgSjAvpjYdJFZMMfWzXrNA==", 1 },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "BD0D84736D24484943A9775935991734D43DCAF53293F3B26DE558FCD358B3B4", "ZgSjAvpjYdJFZMMfWzXrNA==", 2 }
                });

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "Id", "CategoryId", "Description", "Image", "Price", "Stock", "Title" },
                values: new object[,]
                {
                    { 1, 1, "kids dress", "dress1.jpg", 299.99m, (short)10, " Fancy dress" },
                    { 2, 2, "T-Shirt for men", "BlueTShirt.jpg", 199.99m, (short)10, "Blue T-Shirt" },
                    { 3, 1, "Girls skirt", "skirt1.jpg", 159.99m, (short)10, "Skirt" },
                    { 4, 1, "kids jumpersuit", "jumpersuit1.jpg", 279.99m, (short)10, "Jumpersuit" },
                    { 5, 2, "T-Shirt for men", "RedT-Shirt.jpg", 199.99m, (short)10, "Red T-Shirt" },
                    { 6, 3, "Summer clothing", "womendress1.jpg", 299.99m, (short)10, "Long dress" },
                    { 7, 3, "Spring Floral dress for women", "womendress2.jpg", 299.99m, (short)10, "Floral dress" }
                });

            migrationBuilder.InsertData(
                table: "HomeAddress",
                columns: new[] { "Id", "AccountInfoId", "Address", "City", "Country", "PostalCode", "TelePhone" },
                values: new object[] { 1, new Guid("ca635ab3-d525-4e4f-ad94-69b7c6441427"), "Husum", "Copenhagen", "Danmark", "2200", "+228415799" });

            migrationBuilder.InsertData(
                table: "HomeAddress",
                columns: new[] { "Id", "AccountInfoId", "Address", "City", "Country", "PostalCode", "TelePhone" },
                values: new object[] { 2, new Guid("4bb0b59f-30d3-4413-8ad3-36b4e61393cc"), "Gladsaxe", "Copenhagen", "Danmark", "2400", "+228515798" });

            migrationBuilder.CreateIndex(
                name: "IX_AccountInfo_UserId",
                table: "AccountInfo",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_HomeAddress_AccountInfoId",
                table: "HomeAddress",
                column: "AccountInfoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_AccountInfoId",
                table: "Order",
                column: "AccountInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_PaymentId",
                table: "Order",
                column: "PaymentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordEntity_UserId",
                table: "PasswordEntity",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryId",
                table: "Product",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingDetails_OrderId",
                table: "ShippingDetails",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HomeAddress");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "PasswordEntity");

            migrationBuilder.DropTable(
                name: "ShippingDetails");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "AccountInfo");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}

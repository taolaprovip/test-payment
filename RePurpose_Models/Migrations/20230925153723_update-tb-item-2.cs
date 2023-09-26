using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RePurpose_Models.Migrations
{
    public partial class updatetbitem2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Categories_ItemCategory",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_ItemCategory",
                table: "Items");

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "Items",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Items_CategoryId",
                table: "Items",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Categories_CategoryId",
                table: "Items",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Categories_CategoryId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_CategoryId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Items");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemCategory",
                table: "Items",
                column: "ItemCategory");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Categories_ItemCategory",
                table: "Items",
                column: "ItemCategory",
                principalTable: "Categories",
                principalColumn: "Id");
        }
    }
}

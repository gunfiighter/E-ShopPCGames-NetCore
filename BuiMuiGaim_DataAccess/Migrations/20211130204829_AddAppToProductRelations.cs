using Microsoft.EntityFrameworkCore.Migrations;

namespace BuiMuiGaim_DataAccess.Migrations
{
    public partial class AddAppToProductRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApplicationId",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "Product");
        }
    }
}

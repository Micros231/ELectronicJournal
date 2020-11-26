using Microsoft.EntityFrameworkCore.Migrations;

namespace ElectronicJournal.EntityFrameworkCore.Migrations
{
    public partial class AddRoleLocalizedName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LocalizedName",
                table: "AspNetRoles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocalizedName",
                table: "AspNetRoles");
        }
    }
}

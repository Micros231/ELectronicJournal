using Microsoft.EntityFrameworkCore.Migrations;

namespace ElectronicJournal.EntityFrameworkCore.Migrations
{
    public partial class AddFirstNameAndLastName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Teachers_UserId",
                table: "Teachers",
                column: "UserId");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Students_UserId",
                table: "Students",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Teachers_UserId",
                table: "Teachers");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Students_UserId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");
        }
    }
}

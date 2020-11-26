using Microsoft.EntityFrameworkCore.Migrations;

namespace ElectronicJournal.EntityFrameworkCore.Migrations
{
    public partial class SimpleUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeacheId",
                table: "AcademicSubjectScores");

            migrationBuilder.AlterColumn<long>(
                name: "AcademicSubjectId",
                table: "Teachers",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "StudyGroupId",
                table: "Students",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "TeacherId",
                table: "AcademicSubjectScores",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "AcademicSubjectScores");

            migrationBuilder.AlterColumn<long>(
                name: "AcademicSubjectId",
                table: "Teachers",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "StudyGroupId",
                table: "Students",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TeacheId",
                table: "AcademicSubjectScores",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}

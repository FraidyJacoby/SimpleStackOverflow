using Microsoft.EntityFrameworkCore.Migrations;

namespace SimpleStackOverflow.Data.Migrations
{
    public partial class AddUserIdToAnswer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Answers",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Answers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}

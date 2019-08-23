using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainingApplication.Data.Migrations
{
    public partial class Adjustlevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Level",
                table: "Topic",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Level",
                table: "Topic");
        }
    }
}

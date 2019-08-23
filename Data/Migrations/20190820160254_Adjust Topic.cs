using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainingApplication.Data.Migrations
{
    public partial class AdjustTopic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Topic",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Topic");
        }
    }
}

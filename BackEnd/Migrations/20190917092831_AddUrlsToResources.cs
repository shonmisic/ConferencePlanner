using Microsoft.EntityFrameworkCore.Migrations;

namespace BackEnd.Migrations
{
    public partial class AddUrlsToResources : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Sessions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Images",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Conferences",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Attendees",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Conferences");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Attendees");
        }
    }
}

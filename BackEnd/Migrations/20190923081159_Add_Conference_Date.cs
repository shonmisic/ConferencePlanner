using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BackEnd.Migrations
{
    public partial class Add_Conference_Date : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "EndTime",
                table: "Conferences",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "StartTime",
                table: "Conferences",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Conferences");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Conferences");
        }
    }
}

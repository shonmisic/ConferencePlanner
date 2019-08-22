using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BackEnd.Migrations
{
    public partial class AddImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Conferences_ConferenceID",
                table: "Sessions");

            migrationBuilder.RenameColumn(
                name: "ConferenceID",
                table: "Sessions",
                newName: "ConferenceId");

            migrationBuilder.RenameIndex(
                name: "IX_Sessions_ConferenceID",
                table: "Sessions",
                newName: "IX_Sessions_ConferenceId");

            migrationBuilder.AlterColumn<int>(
                name: "ConferenceId",
                table: "Sessions",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    UploadDate = table.Column<DateTimeOffset>(nullable: false),
                    Content = table.Column<string>(nullable: false),
                    AttendeeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Images_Attendees_AttendeeId",
                        column: x => x.AttendeeId,
                        principalTable: "Attendees",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Images_AttendeeId",
                table: "Images",
                column: "AttendeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Conferences_ConferenceId",
                table: "Sessions",
                column: "ConferenceId",
                principalTable: "Conferences",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Conferences_ConferenceId",
                table: "Sessions");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.RenameColumn(
                name: "ConferenceId",
                table: "Sessions",
                newName: "ConferenceID");

            migrationBuilder.RenameIndex(
                name: "IX_Sessions_ConferenceId",
                table: "Sessions",
                newName: "IX_Sessions_ConferenceID");

            migrationBuilder.AlterColumn<int>(
                name: "ConferenceID",
                table: "Sessions",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Conferences_ConferenceID",
                table: "Sessions",
                column: "ConferenceID",
                principalTable: "Conferences",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

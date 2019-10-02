using Microsoft.EntityFrameworkCore.Migrations;

namespace BackEnd.Migrations
{
    public partial class AddImagesToAttendeesAndSpeakers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Attendees_AttendeeId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_AttendeeId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "AttendeeId",
                table: "Images");

            migrationBuilder.CreateTable(
                name: "AttendeeImage",
                columns: table => new
                {
                    AttendeeId = table.Column<int>(nullable: false),
                    ImageId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendeeImage", x => new { x.AttendeeId, x.ImageId });
                    table.ForeignKey(
                        name: "FK_AttendeeImage_Attendees_AttendeeId",
                        column: x => x.AttendeeId,
                        principalTable: "Attendees",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttendeeImage_Images_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Images",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpeakerImage",
                columns: table => new
                {
                    SpeakerId = table.Column<int>(nullable: false),
                    ImageId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpeakerImage", x => new { x.SpeakerId, x.ImageId });
                    table.ForeignKey(
                        name: "FK_SpeakerImage_Images_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Images",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpeakerImage_Speakers_SpeakerId",
                        column: x => x.SpeakerId,
                        principalTable: "Speakers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttendeeImage_ImageId",
                table: "AttendeeImage",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_SpeakerImage_ImageId",
                table: "SpeakerImage",
                column: "ImageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttendeeImage");

            migrationBuilder.DropTable(
                name: "SpeakerImage");

            migrationBuilder.AddColumn<int>(
                name: "AttendeeId",
                table: "Images",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Images_AttendeeId",
                table: "Images",
                column: "AttendeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Attendees_AttendeeId",
                table: "Images",
                column: "AttendeeId",
                principalTable: "Attendees",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

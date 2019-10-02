using Microsoft.EntityFrameworkCore.Migrations;

namespace BackEnd.Migrations
{
    public partial class AddConferenceSpeakerJoinTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Speakers_Conferences_ConferenceID",
                table: "Speakers");

            migrationBuilder.DropIndex(
                name: "IX_Speakers_ConferenceID",
                table: "Speakers");

            migrationBuilder.DropColumn(
                name: "ConferenceID",
                table: "Speakers");

            migrationBuilder.CreateTable(
                name: "ConferenceSpeaker",
                columns: table => new
                {
                    ConferenceId = table.Column<int>(nullable: false),
                    SpeakerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConferenceSpeaker", x => new { x.ConferenceId, x.SpeakerId });
                    table.ForeignKey(
                        name: "FK_ConferenceSpeaker_Conferences_ConferenceId",
                        column: x => x.ConferenceId,
                        principalTable: "Conferences",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConferenceSpeaker_Speakers_SpeakerId",
                        column: x => x.SpeakerId,
                        principalTable: "Speakers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConferenceSpeaker_SpeakerId",
                table: "ConferenceSpeaker",
                column: "SpeakerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConferenceSpeaker");

            migrationBuilder.AddColumn<int>(
                name: "ConferenceID",
                table: "Speakers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Speakers_ConferenceID",
                table: "Speakers",
                column: "ConferenceID");

            migrationBuilder.AddForeignKey(
                name: "FK_Speakers_Conferences_ConferenceID",
                table: "Speakers",
                column: "ConferenceID",
                principalTable: "Conferences",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

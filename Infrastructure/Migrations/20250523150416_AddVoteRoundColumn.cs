using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddVoteRoundColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Votes_UserId",
                table: "Votes");

            migrationBuilder.AddColumn<int>(
                name: "Round",
                table: "Votes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Votes_UserId_VoteDate",
                table: "Votes",
                columns: new[] { "UserId", "VoteDate" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Votes_UserId_VoteDate",
                table: "Votes");

            migrationBuilder.DropColumn(
                name: "Round",
                table: "Votes");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_UserId",
                table: "Votes",
                column: "UserId");
        }
    }
}

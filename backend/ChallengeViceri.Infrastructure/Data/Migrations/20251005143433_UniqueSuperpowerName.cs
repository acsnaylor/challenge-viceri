using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChallengeViceri.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UniqueSuperpowerName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM \"Superpoderes\" a USING \"Superpoderes\" b WHERE a.\"Id\" > b.\"Id\" AND a.\"Superpoder\" = b.\"Superpoder\";");
            migrationBuilder.CreateIndex(
                name: "IX_Superpoderes_Superpoder",
                table: "Superpoderes",
                column: "Superpoder",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Superpoderes_Superpoder",
                table: "Superpoderes");
        }
    }
}

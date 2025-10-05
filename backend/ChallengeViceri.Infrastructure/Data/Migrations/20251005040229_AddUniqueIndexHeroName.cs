using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChallengeViceri.Infrastructure.Data.Migrations
{
    public partial class AddUniqueIndexHeroName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Herois_NomeHeroi",
                table: "Herois",
                column: "NomeHeroi",
                unique: true);
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Herois_NomeHeroi",
                table: "Herois");
        }
    }
}


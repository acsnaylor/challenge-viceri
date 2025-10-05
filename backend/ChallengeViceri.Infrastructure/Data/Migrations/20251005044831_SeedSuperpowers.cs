using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChallengeViceri.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedSuperpowers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Superpoderes",
                columns: new[] { "Superpoder", "Descricao" },
                values: new object[,]
                {
                    { "Força", "Força sobre-humana" },
                    { "Velocidade", "Movimento ultra rápido" },
                    { "Voo", "Capacidade de voar" },
                    { "Invisibilidade", "Ficar invisível" },
                    { "Telepatia", "Ler e influenciar mentes" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM \"Superpoderes\" WHERE \"Superpoder\" IN ('Força','Velocidade','Voo','Invisibilidade','Telepatia')");
        }
    }
}

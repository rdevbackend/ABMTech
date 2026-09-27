using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ABMTech.Api.Migrations
{
    /// <inheritdoc />
    public partial class CriarBancoSqlite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrdensServico",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CodigoRastreio = table.Column<string>(type: "TEXT", nullable: false),
                    NomeCliente = table.Column<string>(type: "TEXT", nullable: false),
                    WhatsappCliente = table.Column<string>(type: "TEXT", nullable: false),
                    ModeloAparelho = table.Column<string>(type: "TEXT", nullable: false),
                    DefeitoRelatado = table.Column<string>(type: "TEXT", nullable: false),
                    PrecoPecaFornecedor = table.Column<decimal>(type: "TEXT", nullable: false),
                    ValorMaoDeObra = table.Column<decimal>(type: "TEXT", nullable: false),
                    MargemLucroPecaPorcentagem = table.Column<decimal>(type: "TEXT", nullable: false),
                    ValorTotalCalculado = table.Column<decimal>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdensServico", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrdensServico");
        }
    }
}

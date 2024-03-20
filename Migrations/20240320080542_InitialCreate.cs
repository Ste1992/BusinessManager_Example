using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessManager.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dipendenti",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Cognome = table.Column<string>(type: "TEXT", nullable: false),
                    Login = table.Column<string>(type: "TEXT", nullable: false),
                    Inquadramento = table.Column<string>(type: "TEXT", nullable: false),
                    Indirizzo = table.Column<string>(type: "TEXT", nullable: false),
                    NumeroCivico = table.Column<int>(type: "INTEGER", nullable: false),
                    Cap = table.Column<int>(type: "INTEGER", nullable: false),
                    Comune = table.Column<string>(type: "TEXT", nullable: false),
                    Provincia = table.Column<string>(type: "TEXT", nullable: false),
                    LivelloContratto = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dipendenti", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BustaPaga",
                columns: table => new
                {
                    DipendenteId = table.Column<int>(type: "INTEGER", nullable: false),
                    Index = table.Column<int>(type: "INTEGER", nullable: false),
                    OreLavorate = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    PagaOraria = table.Column<decimal>(type: "TEXT", nullable: false),
                    OreStraordinario = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    PagaStraordinario = table.Column<decimal>(type: "TEXT", nullable: false),
                    TotalePaga = table.Column<decimal>(type: "TEXT", nullable: false),
                    DataEmissione = table.Column<DateOnly>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BustaPaga", x => x.DipendenteId);
                    table.ForeignKey(
                        name: "FK_BustaPaga_Dipendenti_DipendenteId",
                        column: x => x.DipendenteId,
                        principalTable: "Dipendenti",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Timbrature",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DipendenteId = table.Column<int>(type: "INTEGER", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TipoPresenza = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timbrature", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Timbrature_Dipendenti_DipendenteId",
                        column: x => x.DipendenteId,
                        principalTable: "Dipendenti",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Timbrature_DipendenteId",
                table: "Timbrature",
                column: "DipendenteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BustaPaga");

            migrationBuilder.DropTable(
                name: "Timbrature");

            migrationBuilder.DropTable(
                name: "Dipendenti");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DynamicFormsAPI.Migrations
{
    /// <inheritdoc />
    public partial class AgregarRespuesta1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Respuestas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormularioId = table.Column<int>(type: "int", nullable: false),
                    CampoId = table.Column<int>(type: "int", nullable: false),
                    Valor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Respuestas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Respuestas_Campos_CampoId",
                        column: x => x.CampoId,
                        principalTable: "Campos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Respuestas_Formularios_FormularioId",
                        column: x => x.FormularioId,
                        principalTable: "Formularios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Respuestas_CampoId",
                table: "Respuestas",
                column: "CampoId");

            migrationBuilder.CreateIndex(
                name: "IX_Respuestas_FormularioId",
                table: "Respuestas",
                column: "FormularioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Respuestas");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Terapias_rehabilitaciones.Migrations
{
    /// <inheritdoc />
    public partial class CorregirPlanTratamiento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Planes_Pacientes_PacienteId",
                table: "Planes");

            migrationBuilder.DropIndex(
                name: "IX_Planes_PacienteId",
                table: "Planes");

            migrationBuilder.DropColumn(
                name: "PacienteId",
                table: "Planes");

            migrationBuilder.CreateIndex(
                name: "IX_Planes_IdPaciente",
                table: "Planes",
                column: "IdPaciente");

            migrationBuilder.AddForeignKey(
                name: "FK_Planes_Pacientes_IdPaciente",
                table: "Planes",
                column: "IdPaciente",
                principalTable: "Pacientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Planes_Pacientes_IdPaciente",
                table: "Planes");

            migrationBuilder.DropIndex(
                name: "IX_Planes_IdPaciente",
                table: "Planes");

            migrationBuilder.AddColumn<int>(
                name: "PacienteId",
                table: "Planes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Planes_PacienteId",
                table: "Planes",
                column: "PacienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Planes_Pacientes_PacienteId",
                table: "Planes",
                column: "PacienteId",
                principalTable: "Pacientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

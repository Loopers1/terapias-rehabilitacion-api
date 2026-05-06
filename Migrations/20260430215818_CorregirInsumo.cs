using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Terapias_rehabilitaciones.Migrations
{
    /// <inheritdoc />
    public partial class CorregirInsumo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsoInsumos_Insumos_InsumoId",
                table: "UsoInsumos");

            migrationBuilder.DropForeignKey(
                name: "FK_UsoInsumos_Sesiones_SesionId",
                table: "UsoInsumos");

            migrationBuilder.DropIndex(
                name: "IX_UsoInsumos_InsumoId",
                table: "UsoInsumos");

            migrationBuilder.DropIndex(
                name: "IX_UsoInsumos_SesionId",
                table: "UsoInsumos");

            migrationBuilder.DropColumn(
                name: "InsumoId",
                table: "UsoInsumos");

            migrationBuilder.DropColumn(
                name: "SesionId",
                table: "UsoInsumos");

            migrationBuilder.CreateIndex(
                name: "IX_UsoInsumos_IdInsumo",
                table: "UsoInsumos",
                column: "IdInsumo");

            migrationBuilder.CreateIndex(
                name: "IX_UsoInsumos_IdSesion",
                table: "UsoInsumos",
                column: "IdSesion");

            migrationBuilder.AddForeignKey(
                name: "FK_UsoInsumos_Insumos_IdInsumo",
                table: "UsoInsumos",
                column: "IdInsumo",
                principalTable: "Insumos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsoInsumos_Sesiones_IdSesion",
                table: "UsoInsumos",
                column: "IdSesion",
                principalTable: "Sesiones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsoInsumos_Insumos_IdInsumo",
                table: "UsoInsumos");

            migrationBuilder.DropForeignKey(
                name: "FK_UsoInsumos_Sesiones_IdSesion",
                table: "UsoInsumos");

            migrationBuilder.DropIndex(
                name: "IX_UsoInsumos_IdInsumo",
                table: "UsoInsumos");

            migrationBuilder.DropIndex(
                name: "IX_UsoInsumos_IdSesion",
                table: "UsoInsumos");

            migrationBuilder.AddColumn<int>(
                name: "InsumoId",
                table: "UsoInsumos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SesionId",
                table: "UsoInsumos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UsoInsumos_InsumoId",
                table: "UsoInsumos",
                column: "InsumoId");

            migrationBuilder.CreateIndex(
                name: "IX_UsoInsumos_SesionId",
                table: "UsoInsumos",
                column: "SesionId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsoInsumos_Insumos_InsumoId",
                table: "UsoInsumos",
                column: "InsumoId",
                principalTable: "Insumos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsoInsumos_Sesiones_SesionId",
                table: "UsoInsumos",
                column: "SesionId",
                principalTable: "Sesiones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

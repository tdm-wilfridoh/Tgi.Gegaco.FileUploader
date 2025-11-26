using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tgi.Gegaco.FileUploader.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CambiarNombreTamaño : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Tamaño",
                table: "Documentos",
                newName: "Tamano");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Tamano",
                table: "Documentos",
                newName: "Tamaño");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dystopian_Civil_Office.Migrations
{
    /// <inheritdoc />
    public partial class AddPaperlessDocumentIdReferenceInDocuments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "paperless_document_id",
                table: "documents",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "paperless_document_id",
                table: "documents");
        }
    }
}

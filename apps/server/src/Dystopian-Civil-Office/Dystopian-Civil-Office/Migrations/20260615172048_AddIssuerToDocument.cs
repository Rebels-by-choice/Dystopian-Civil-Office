using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dystopian_Civil_Office.Migrations
{
    /// <inheritdoc />
    public partial class AddIssuerToDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "document_issuer_id",
                table: "documents",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ix_documents_document_issuer_id",
                table: "documents",
                column: "document_issuer_id");

            migrationBuilder.AddForeignKey(
                name: "fk_documents_persons_document_issuer_id",
                table: "documents",
                column: "document_issuer_id",
                principalTable: "persons",
                principalColumn: "person_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_documents_persons_document_issuer_id",
                table: "documents");

            migrationBuilder.DropIndex(
                name: "ix_documents_document_issuer_id",
                table: "documents");

            migrationBuilder.DropColumn(
                name: "document_issuer_id",
                table: "documents");
        }
    }
}

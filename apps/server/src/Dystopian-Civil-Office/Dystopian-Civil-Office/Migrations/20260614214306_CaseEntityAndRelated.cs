using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Dystopian_Civil_Office.Migrations
{
    /// <inheritdoc />
    public partial class CaseEntityAndRelated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_functionary",
                table: "persons",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "case_id",
                table: "documents",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "cases",
                columns: table => new
                {
                    case_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    status = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    closed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    initiator_id = table.Column<int>(type: "integer", nullable: false),
                    responder_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_case", x => x.case_id);
                    table.ForeignKey(
                        name: "fk_case_persons_initiator_id",
                        column: x => x.initiator_id,
                        principalTable: "persons",
                        principalColumn: "person_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_case_persons_responder_id",
                        column: x => x.responder_id,
                        principalTable: "persons",
                        principalColumn: "person_id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_documents_case_id",
                table: "documents",
                column: "case_id");

            migrationBuilder.CreateIndex(
                name: "ix_case_initiator_id",
                table: "cases",
                column: "initiator_id");

            migrationBuilder.CreateIndex(
                name: "ix_case_responder_id",
                table: "cases",
                column: "responder_id");

            migrationBuilder.AddForeignKey(
                name: "fk_documents_case_case_id",
                table: "documents",
                column: "case_id",
                principalTable: "cases",
                principalColumn: "case_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_documents_case_case_id",
                table: "documents");

            migrationBuilder.DropTable(
                name: "cases");

            migrationBuilder.DropIndex(
                name: "ix_documents_case_id",
                table: "documents");

            migrationBuilder.DropColumn(
                name: "is_functionary",
                table: "persons");

            migrationBuilder.DropColumn(
                name: "case_id",
                table: "documents");
        }
    }
}

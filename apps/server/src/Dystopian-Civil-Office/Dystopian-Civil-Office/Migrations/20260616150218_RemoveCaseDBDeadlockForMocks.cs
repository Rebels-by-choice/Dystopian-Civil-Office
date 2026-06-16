using Microsoft.EntityFrameworkCore.Migrations;
using static Dystopian_Civil_Office.Migrations.MigrationHelpers;

#nullable disable

namespace Dystopian_Civil_Office.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCaseDBDeadlockForMocks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_case_persons_initiator_id",
                table: "cases");
            
            migrationBuilder.AlterColumn<int>( // Change <int> to <Guid> or <long> if that matches your ID type
                name: "initiator_id",
                table: "cases",
                type: "integer",            // Match your exact DB type (e.g., "integer", "uuid", "bigint")
                nullable: true,             // This makes the key optional
                oldClrType: typeof(int),
                oldType: "integer");

            // migrationBuilder.DropForeignKey(
            //     name: "fk_case_persons_responder_id",
            //     table: "case");
            //
            // migrationBuilder.DropForeignKey(
            //     name: "fk_documents_case_case_id",
            //     table: "documents");
            //
            // migrationBuilder.DropForeignKey(
            //     name: "fk_persons_addresses_address_id",
            //     table: "persons");
            //
            // migrationBuilder.DropPrimaryKey(
            //     name: "pk_case",
            //     table: "case");
            //
            // migrationBuilder.RenameTable(
            //     name: "case",
            //     newName: "cases");
            //
            // migrationBuilder.RenameIndex(
            //     name: "IX_documents_name",
            //     table: "documents",
            //     newName: "ix_documents_name");
            //
            // migrationBuilder.RenameIndex(
            //     name: "IX_addresses_registry_number",
            //     table: "addresses",
            //     newName: "ix_addresses_registry_number");
            //
            // migrationBuilder.RenameIndex(
            //     name: "ix_case_responder_id",
            //     table: "cases",
            //     newName: "ix_cases_responder_id");
            //
            // migrationBuilder.RenameIndex(
            //     name: "ix_case_initiator_id",
            //     table: "cases",
            //     newName: "ix_cases_initiator_id");
            //
            // migrationBuilder.AddPrimaryKey(
            //     name: "pk_cases",
            //     table: "cases",
            //     column: "case_id");

            migrationBuilder.AddForeignKey(
                name: "fk_cases_persons_initiator_id",
                table: "cases",
                column: "initiator_id",
                principalTable: "persons",
                principalColumn: "person_id",
                onDelete: ReferentialAction.SetNull);
            //
            // migrationBuilder.AddForeignKey(
            //     name: "fk_cases_persons_responder_id",
            //     table: "cases",
            //     column: "responder_id",
            //     principalTable: "persons",
            //     principalColumn: "person_id",
            //     onDelete: ReferentialAction.Restrict);
            //
            // migrationBuilder.AddForeignKey(
            //     name: "fk_documents_cases_case_id",
            //     table: "documents",
            //     column: "case_id",
            //     principalTable: "cases",
            //     principalColumn: "case_id",
            //     onDelete: ReferentialAction.Cascade);
            //
            // migrationBuilder.AddForeignKey(
            //     name: "fk_persons_addresses_address_id",
            //     table: "persons",
            //     column: "address_id",
            //     principalTable: "addresses",
            //     principalColumn: "address_id",
            //     onDelete: ReferentialAction.Cascade);
            
            ExecuteEmbeddedSql(migrationBuilder, "Dystopian_Civil_Office.InitDb.Sql.Procedures.cases.procedures.init.sql");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_cases_persons_initiator_id",
                table: "cases");
            
            migrationBuilder.AlterColumn<int>(
                name: "initiator_id",
                table: "cases",
                type: "integer",
                nullable: false,            // Back to required
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
            
            //
            // migrationBuilder.DropForeignKey(
            //     name: "fk_cases_persons_responder_id",
            //     table: "cases");
            //
            // migrationBuilder.DropForeignKey(
            //     name: "fk_documents_cases_case_id",
            //     table: "documents");
            //
            // migrationBuilder.DropForeignKey(
            //     name: "fk_persons_addresses_address_id",
            //     table: "persons");
            //
            // migrationBuilder.DropPrimaryKey(
            //     name: "pk_cases",
            //     table: "cases");
            //
            // migrationBuilder.RenameTable(
            //     name: "cases",
            //     newName: "case");
            //
            // migrationBuilder.RenameIndex(
            //     name: "ix_documents_name",
            //     table: "documents",
            //     newName: "IX_documents_name");
            //
            // migrationBuilder.RenameIndex(
            //     name: "ix_addresses_registry_number",
            //     table: "addresses",
            //     newName: "IX_addresses_registry_number");
            //
            // migrationBuilder.RenameIndex(
            //     name: "ix_cases_responder_id",
            //     table: "case",
            //     newName: "ix_case_responder_id");
            //
            // migrationBuilder.RenameIndex(
            //     name: "ix_cases_initiator_id",
            //     table: "case",
            //     newName: "ix_case_initiator_id");
            //
            // migrationBuilder.AddPrimaryKey(
            //     name: "pk_case",
            //     table: "case",
            //     column: "case_id");

            migrationBuilder.AddForeignKey(
                name: "fk_case_persons_initiator_id",
                table: "cases",
                column: "initiator_id",
                principalTable: "persons",
                principalColumn: "person_id",
                onDelete: ReferentialAction.Cascade);

        //     migrationBuilder.AddForeignKey(
        //         name: "fk_case_persons_responder_id",
        //         table: "case",
        //         column: "responder_id",
        //         principalTable: "persons",
        //         principalColumn: "person_id");
        //
        //     migrationBuilder.AddForeignKey(
        //         name: "fk_documents_case_case_id",
        //         table: "documents",
        //         column: "case_id",
        //         principalTable: "case",
        //         principalColumn: "case_id",
        //         onDelete: ReferentialAction.Cascade);
        //
        //     migrationBuilder.AddForeignKey(
        //         name: "fk_persons_addresses_address_id",
        //         table: "persons",
        //         column: "address_id",
        //         principalTable: "addresses",
        //         principalColumn: "address_id",
        //         onDelete: ReferentialAction.Restrict);
        }
    }
}

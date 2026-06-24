using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dystopian_Civil_Office.Migrations
{
    /// <inheritdoc />
    public partial class PascalCaseToSnakeCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_addresses_documents_document_id",
                table: "addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_birth_records_documents_document_id",
                table: "birth_records");

            migrationBuilder.DropForeignKey(
                name: "FK_birth_records_persons_father_id",
                table: "birth_records");

            migrationBuilder.DropForeignKey(
                name: "FK_birth_records_persons_mother_id",
                table: "birth_records");

            migrationBuilder.DropForeignKey(
                name: "FK_birth_records_persons_person_id",
                table: "birth_records");

            migrationBuilder.DropForeignKey(
                name: "FK_death_records_documents_document_id",
                table: "death_records");

            migrationBuilder.DropForeignKey(
                name: "FK_death_records_persons_person_id",
                table: "death_records");

            migrationBuilder.DropForeignKey(
                name: "FK_marriage_records_documents_document_id",
                table: "marriage_records");

            migrationBuilder.DropForeignKey(
                name: "FK_marriage_records_persons_spouse1_id",
                table: "marriage_records");

            migrationBuilder.DropForeignKey(
                name: "FK_marriage_records_persons_spouse2_id",
                table: "marriage_records");

            migrationBuilder.DropForeignKey(
                name: "FK_persons_addresses_address_id",
                table: "persons");

            migrationBuilder.DropForeignKey(
                name: "FK_persons_documents_document_id",
                table: "persons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_persons",
                table: "persons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_person_archives",
                table: "person_archives");

            migrationBuilder.DropPrimaryKey(
                name: "PK_marriage_records",
                table: "marriage_records");

            migrationBuilder.DropPrimaryKey(
                name: "PK_marriage_record_archives",
                table: "marriage_record_archives");

            migrationBuilder.DropPrimaryKey(
                name: "PK_documents",
                table: "documents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_document_archives",
                table: "document_archives");

            migrationBuilder.DropPrimaryKey(
                name: "PK_death_records",
                table: "death_records");

            migrationBuilder.DropPrimaryKey(
                name: "PK_death_record_archives",
                table: "death_record_archives");

            migrationBuilder.DropPrimaryKey(
                name: "PK_birth_records",
                table: "birth_records");

            migrationBuilder.DropPrimaryKey(
                name: "PK_birth_record_archives",
                table: "birth_record_archives");

            migrationBuilder.DropPrimaryKey(
                name: "PK_addresses",
                table: "addresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_address_archives",
                table: "address_archives");

            migrationBuilder.RenameIndex(
                name: "IX_persons_pesel",
                table: "persons",
                newName: "ix_persons_pesel");

            migrationBuilder.RenameIndex(
                name: "IX_persons_document_id",
                table: "persons",
                newName: "ix_persons_document_id");

            migrationBuilder.RenameIndex(
                name: "IX_persons_address_id",
                table: "persons",
                newName: "ix_persons_address_id");

            // DONT UNCOMMENT
            // DONT UNCOMMENT
            // DONT UNCOMMENT
            // DONT UNCOMMENT
            // DONT UNCOMMENT
            // migrationBuilder.RenameColumn(
            //     name: "Gender",
            //     table: "person_archives",
            //     newName: "gender");
            //
            // migrationBuilder.RenameColumn(
            //     name: "PersonPesel",
            //     table: "person_archives",
            //     newName: "person_pesel");
            //
            // migrationBuilder.RenameColumn(
            //     name: "MiddleName",
            //     table: "person_archives",
            //     newName: "middle_name");
            //
            // migrationBuilder.RenameColumn(
            //     name: "LastName",
            //     table: "person_archives",
            //     newName: "last_name");
            //
            // migrationBuilder.RenameColumn(
            //     name: "FirstName",
            //     table: "person_archives",
            //     newName: "first_name");
            //
            // migrationBuilder.RenameColumn(
            //     name: "DocumentName",
            //     table: "person_archives",
            //     newName: "document_name");
            //
            // migrationBuilder.RenameColumn(
            //     name: "DeletedAt",
            //     table: "person_archives",
            //     newName: "deleted_at");
            //
            // migrationBuilder.RenameColumn(
            //     name: "BirthPlace",
            //     table: "person_archives",
            //     newName: "birth_place");
            //
            // migrationBuilder.RenameColumn(
            //     name: "BirthDate",
            //     table: "person_archives",
            //     newName: "birth_date");
            //
            // migrationBuilder.RenameColumn(
            //     name: "PersonArchiveId",
            //     table: "person_archives",
            //     newName: "person_archive_id");

            migrationBuilder.RenameIndex(
                name: "IX_marriage_records_spouse2_id",
                table: "marriage_records",
                newName: "ix_marriage_records_spouse2_id");

            migrationBuilder.RenameIndex(
                name: "IX_marriage_records_spouse1_id",
                table: "marriage_records",
                newName: "ix_marriage_records_spouse1_id");

            migrationBuilder.RenameIndex(
                name: "IX_marriage_records_registry_number",
                table: "marriage_records",
                newName: "ix_marriage_records_registry_number");

            migrationBuilder.RenameIndex(
                name: "IX_marriage_records_document_id",
                table: "marriage_records",
                newName: "ix_marriage_records_document_id");

            // migrationBuilder.RenameColumn(
            //     name: "Spouse2Pesel",
            //     table: "marriage_record_archives",
            //     newName: "spouse2pesel");
            //
            // migrationBuilder.RenameColumn(
            //     name: "Spouse1Pesel",
            //     table: "marriage_record_archives",
            //     newName: "spouse1pesel");
            //
            // migrationBuilder.RenameColumn(
            //     name: "RegistryNumber",
            //     table: "marriage_record_archives",
            //     newName: "registry_number");
            //
            // migrationBuilder.RenameColumn(
            //     name: "RegistryDate",
            //     table: "marriage_record_archives",
            //     newName: "registry_date");
            //
            // migrationBuilder.RenameColumn(
            //     name: "MarriagePlace",
            //     table: "marriage_record_archives",
            //     newName: "marriage_place");
            //
            // migrationBuilder.RenameColumn(
            //     name: "MarriageDate",
            //     table: "marriage_record_archives",
            //     newName: "marriage_date");
            //
            // migrationBuilder.RenameColumn(
            //     name: "DocumentName",
            //     table: "marriage_record_archives",
            //     newName: "document_name");
            //
            // migrationBuilder.RenameColumn(
            //     name: "DeletedAt",
            //     table: "marriage_record_archives",
            //     newName: "deleted_at");
            //
            // migrationBuilder.RenameColumn(
            //     name: "MarriageRecordArchiveId",
            //     table: "marriage_record_archives",
            //     newName: "marriage_record_archive_id");
            //
            // migrationBuilder.RenameColumn(
            //     name: "Name",
            //     table: "document_archives",
            //     newName: "name");
            //
            // migrationBuilder.RenameColumn(
            //     name: "Category",
            //     table: "document_archives",
            //     newName: "category");
            //
            // migrationBuilder.RenameColumn(
            //     name: "ImportDate",
            //     table: "document_archives",
            //     newName: "import_date");
            //
            // migrationBuilder.RenameColumn(
            //     name: "DeletedAt",
            //     table: "document_archives",
            //     newName: "deleted_at");
            //
            // migrationBuilder.RenameColumn(
            //     name: "DocumentArchiveId",
            //     table: "document_archives",
            //     newName: "document_archive_id");

            migrationBuilder.RenameIndex(
                name: "IX_death_records_registry_number",
                table: "death_records",
                newName: "ix_death_records_registry_number");

            migrationBuilder.RenameIndex(
                name: "IX_death_records_person_id",
                table: "death_records",
                newName: "ix_death_records_person_id");

            migrationBuilder.RenameIndex(
                name: "IX_death_records_document_id",
                table: "death_records",
                newName: "ix_death_records_document_id");

            // migrationBuilder.RenameColumn(
            //     name: "RegistryNumber",
            //     table: "death_record_archives",
            //     newName: "registry_number");
            //
            // migrationBuilder.RenameColumn(
            //     name: "RegistryDate",
            //     table: "death_record_archives",
            //     newName: "registry_date");
            //
            // migrationBuilder.RenameColumn(
            //     name: "PersonPesel",
            //     table: "death_record_archives",
            //     newName: "person_pesel");
            //
            // migrationBuilder.RenameColumn(
            //     name: "DocumentName",
            //     table: "death_record_archives",
            //     newName: "document_name");
            //
            // migrationBuilder.RenameColumn(
            //     name: "DeletedAt",
            //     table: "death_record_archives",
            //     newName: "deleted_at");
            //
            // migrationBuilder.RenameColumn(
            //     name: "DeathPlace",
            //     table: "death_record_archives",
            //     newName: "death_place");
            //
            // migrationBuilder.RenameColumn(
            //     name: "DeathDate",
            //     table: "death_record_archives",
            //     newName: "death_date");
            //
            // migrationBuilder.RenameColumn(
            //     name: "CauseOfDeath",
            //     table: "death_record_archives",
            //     newName: "cause_of_death");
            //
            // migrationBuilder.RenameColumn(
            //     name: "DeathRecordArchiveId",
            //     table: "death_record_archives",
            //     newName: "death_record_archive_id");

            migrationBuilder.RenameIndex(
                name: "IX_birth_records_registry_number",
                table: "birth_records",
                newName: "ix_birth_records_registry_number");

            migrationBuilder.RenameIndex(
                name: "IX_birth_records_person_id",
                table: "birth_records",
                newName: "ix_birth_records_person_id");

            migrationBuilder.RenameIndex(
                name: "IX_birth_records_mother_id",
                table: "birth_records",
                newName: "ix_birth_records_mother_id");

            migrationBuilder.RenameIndex(
                name: "IX_birth_records_father_id",
                table: "birth_records",
                newName: "ix_birth_records_father_id");

            migrationBuilder.RenameIndex(
                name: "IX_birth_records_document_id",
                table: "birth_records",
                newName: "ix_birth_records_document_id");

            // migrationBuilder.RenameColumn(
            //     name: "RegistryNumber",
            //     table: "birth_record_archives",
            //     newName: "registry_number");
            //
            // migrationBuilder.RenameColumn(
            //     name: "RegistryDate",
            //     table: "birth_record_archives",
            //     newName: "registry_date");
            //
            // migrationBuilder.RenameColumn(
            //     name: "MotherPesel",
            //     table: "birth_record_archives",
            //     newName: "mother_pesel");
            //
            // migrationBuilder.RenameColumn(
            //     name: "FatherPesel",
            //     table: "birth_record_archives",
            //     newName: "father_pesel");
            //
            // migrationBuilder.RenameColumn(
            //     name: "DocumentName",
            //     table: "birth_record_archives",
            //     newName: "document_name");
            //
            // migrationBuilder.RenameColumn(
            //     name: "DeletedAt",
            //     table: "birth_record_archives",
            //     newName: "deleted_at");
            //
            // migrationBuilder.RenameColumn(
            //     name: "BornPersonPesel",
            //     table: "birth_record_archives",
            //     newName: "born_person_pesel");
            //
            // migrationBuilder.RenameColumn(
            //     name: "BirthPlace",
            //     table: "birth_record_archives",
            //     newName: "birth_place");
            //
            // migrationBuilder.RenameColumn(
            //     name: "BirthDate",
            //     table: "birth_record_archives",
            //     newName: "birth_date");
            //
            // migrationBuilder.RenameColumn(
            //     name: "BirthRecordArchiveId",
            //     table: "birth_record_archives",
            //     newName: "birth_record_archive_id");

            migrationBuilder.RenameIndex(
                name: "IX_addresses_document_id",
                table: "addresses",
                newName: "ix_addresses_document_id");

            // migrationBuilder.RenameColumn(
            //     name: "Street",
            //     table: "address_archives",
            //     newName: "street");
            //
            // migrationBuilder.RenameColumn(
            //     name: "Country",
            //     table: "address_archives",
            //     newName: "country");
            //
            // migrationBuilder.RenameColumn(
            //     name: "City",
            //     table: "address_archives",
            //     newName: "city");
            //
            // migrationBuilder.RenameColumn(
            //     name: "PostalCode",
            //     table: "address_archives",
            //     newName: "postal_code");
            //
            // migrationBuilder.RenameColumn(
            //     name: "HouseNumber",
            //     table: "address_archives",
            //     newName: "house_number");
            //
            // migrationBuilder.RenameColumn(
            //     name: "DocumentName",
            //     table: "address_archives",
            //     newName: "document_name");
            //
            // migrationBuilder.RenameColumn(
            //     name: "DeletedAt",
            //     table: "address_archives",
            //     newName: "deleted_at");
            //
            // migrationBuilder.RenameColumn(
            //     name: "ApartmentNumber",
            //     table: "address_archives",
            //     newName: "apartment_number");
            //
            // migrationBuilder.RenameColumn(
            //     name: "AddressArchiveId",
            //     table: "address_archives",
            //     newName: "address_archive_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_persons",
                table: "persons",
                column: "person_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_person_archives",
                table: "person_archives",
                column: "person_archive_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_marriage_records",
                table: "marriage_records",
                column: "marriage_record_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_marriage_record_archives",
                table: "marriage_record_archives",
                column: "marriage_record_archive_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_documents",
                table: "documents",
                column: "document_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_document_archives",
                table: "document_archives",
                column: "document_archive_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_death_records",
                table: "death_records",
                column: "death_record_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_death_record_archives",
                table: "death_record_archives",
                column: "death_record_archive_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_birth_records",
                table: "birth_records",
                column: "birth_record_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_birth_record_archives",
                table: "birth_record_archives",
                column: "birth_record_archive_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_addresses",
                table: "addresses",
                column: "address_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_address_archives",
                table: "address_archives",
                column: "address_archive_id");

            migrationBuilder.AddForeignKey(
                name: "fk_addresses_documents_document_id",
                table: "addresses",
                column: "document_id",
                principalTable: "documents",
                principalColumn: "document_id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_birth_records_documents_document_id",
                table: "birth_records",
                column: "document_id",
                principalTable: "documents",
                principalColumn: "document_id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_birth_records_persons_father_id",
                table: "birth_records",
                column: "father_id",
                principalTable: "persons",
                principalColumn: "person_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_birth_records_persons_mother_id",
                table: "birth_records",
                column: "mother_id",
                principalTable: "persons",
                principalColumn: "person_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_birth_records_persons_person_id",
                table: "birth_records",
                column: "person_id",
                principalTable: "persons",
                principalColumn: "person_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_death_records_documents_document_id",
                table: "death_records",
                column: "document_id",
                principalTable: "documents",
                principalColumn: "document_id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_death_records_persons_person_id",
                table: "death_records",
                column: "person_id",
                principalTable: "persons",
                principalColumn: "person_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_marriage_records_documents_document_id",
                table: "marriage_records",
                column: "document_id",
                principalTable: "documents",
                principalColumn: "document_id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_marriage_records_persons_spouse1_id",
                table: "marriage_records",
                column: "spouse1_id",
                principalTable: "persons",
                principalColumn: "person_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_marriage_records_persons_spouse2_id",
                table: "marriage_records",
                column: "spouse2_id",
                principalTable: "persons",
                principalColumn: "person_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_persons_addresses_address_id",
                table: "persons",
                column: "address_id",
                principalTable: "addresses",
                principalColumn: "address_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_persons_documents_document_id",
                table: "persons",
                column: "document_id",
                principalTable: "documents",
                principalColumn: "document_id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_addresses_documents_document_id",
                table: "addresses");

            migrationBuilder.DropForeignKey(
                name: "fk_birth_records_documents_document_id",
                table: "birth_records");

            migrationBuilder.DropForeignKey(
                name: "fk_birth_records_persons_father_id",
                table: "birth_records");

            migrationBuilder.DropForeignKey(
                name: "fk_birth_records_persons_mother_id",
                table: "birth_records");

            migrationBuilder.DropForeignKey(
                name: "fk_birth_records_persons_person_id",
                table: "birth_records");

            migrationBuilder.DropForeignKey(
                name: "fk_death_records_documents_document_id",
                table: "death_records");

            migrationBuilder.DropForeignKey(
                name: "fk_death_records_persons_person_id",
                table: "death_records");

            migrationBuilder.DropForeignKey(
                name: "fk_marriage_records_documents_document_id",
                table: "marriage_records");

            migrationBuilder.DropForeignKey(
                name: "fk_marriage_records_persons_spouse1_id",
                table: "marriage_records");

            migrationBuilder.DropForeignKey(
                name: "fk_marriage_records_persons_spouse2_id",
                table: "marriage_records");

            migrationBuilder.DropForeignKey(
                name: "fk_persons_addresses_address_id",
                table: "persons");

            migrationBuilder.DropForeignKey(
                name: "fk_persons_documents_document_id",
                table: "persons");

            migrationBuilder.DropPrimaryKey(
                name: "pk_persons",
                table: "persons");

            migrationBuilder.DropPrimaryKey(
                name: "pk_person_archives",
                table: "person_archives");

            migrationBuilder.DropPrimaryKey(
                name: "pk_marriage_records",
                table: "marriage_records");

            migrationBuilder.DropPrimaryKey(
                name: "pk_marriage_record_archives",
                table: "marriage_record_archives");

            migrationBuilder.DropPrimaryKey(
                name: "pk_documents",
                table: "documents");

            migrationBuilder.DropPrimaryKey(
                name: "pk_document_archives",
                table: "document_archives");

            migrationBuilder.DropPrimaryKey(
                name: "pk_death_records",
                table: "death_records");

            migrationBuilder.DropPrimaryKey(
                name: "pk_death_record_archives",
                table: "death_record_archives");

            migrationBuilder.DropPrimaryKey(
                name: "pk_birth_records",
                table: "birth_records");

            migrationBuilder.DropPrimaryKey(
                name: "pk_birth_record_archives",
                table: "birth_record_archives");

            migrationBuilder.DropPrimaryKey(
                name: "pk_addresses",
                table: "addresses");

            migrationBuilder.DropPrimaryKey(
                name: "pk_address_archives",
                table: "address_archives");

            migrationBuilder.RenameIndex(
                name: "ix_persons_pesel",
                table: "persons",
                newName: "IX_persons_pesel");

            migrationBuilder.RenameIndex(
                name: "ix_persons_document_id",
                table: "persons",
                newName: "IX_persons_document_id");

            migrationBuilder.RenameIndex(
                name: "ix_persons_address_id",
                table: "persons",
                newName: "IX_persons_address_id");

            // migrationBuilder.RenameColumn(
            //     name: "gender",
            //     table: "person_archives",
            //     newName: "Gender");
            //
            // migrationBuilder.RenameColumn(
            //     name: "person_pesel",
            //     table: "person_archives",
            //     newName: "PersonPesel");
            //
            // migrationBuilder.RenameColumn(
            //     name: "middle_name",
            //     table: "person_archives",
            //     newName: "MiddleName");
            //
            // migrationBuilder.RenameColumn(
            //     name: "last_name",
            //     table: "person_archives",
            //     newName: "LastName");
            //
            // migrationBuilder.RenameColumn(
            //     name: "first_name",
            //     table: "person_archives",
            //     newName: "FirstName");
            //
            // migrationBuilder.RenameColumn(
            //     name: "document_name",
            //     table: "person_archives",
            //     newName: "DocumentName");
            //
            // migrationBuilder.RenameColumn(
            //     name: "deleted_at",
            //     table: "person_archives",
            //     newName: "DeletedAt");
            //
            // migrationBuilder.RenameColumn(
            //     name: "birth_place",
            //     table: "person_archives",
            //     newName: "BirthPlace");
            //
            // migrationBuilder.RenameColumn(
            //     name: "birth_date",
            //     table: "person_archives",
            //     newName: "BirthDate");
            //
            // migrationBuilder.RenameColumn(
            //     name: "person_archive_id",
            //     table: "person_archives",
            //     newName: "PersonArchiveId");

            migrationBuilder.RenameIndex(
                name: "ix_marriage_records_spouse2_id",
                table: "marriage_records",
                newName: "IX_marriage_records_spouse2_id");

            migrationBuilder.RenameIndex(
                name: "ix_marriage_records_spouse1_id",
                table: "marriage_records",
                newName: "IX_marriage_records_spouse1_id");

            migrationBuilder.RenameIndex(
                name: "ix_marriage_records_registry_number",
                table: "marriage_records",
                newName: "IX_marriage_records_registry_number");

            migrationBuilder.RenameIndex(
                name: "ix_marriage_records_document_id",
                table: "marriage_records",
                newName: "IX_marriage_records_document_id");

            // migrationBuilder.RenameColumn(
            //     name: "spouse2pesel",
            //     table: "marriage_record_archives",
            //     newName: "Spouse2Pesel");
            //
            // migrationBuilder.RenameColumn(
            //     name: "spouse1pesel",
            //     table: "marriage_record_archives",
            //     newName: "Spouse1Pesel");
            //
            // migrationBuilder.RenameColumn(
            //     name: "registry_number",
            //     table: "marriage_record_archives",
            //     newName: "RegistryNumber");
            //
            // migrationBuilder.RenameColumn(
            //     name: "registry_date",
            //     table: "marriage_record_archives",
            //     newName: "RegistryDate");
            //
            // migrationBuilder.RenameColumn(
            //     name: "marriage_place",
            //     table: "marriage_record_archives",
            //     newName: "MarriagePlace");
            //
            // migrationBuilder.RenameColumn(
            //     name: "marriage_date",
            //     table: "marriage_record_archives",
            //     newName: "MarriageDate");
            //
            // migrationBuilder.RenameColumn(
            //     name: "document_name",
            //     table: "marriage_record_archives",
            //     newName: "DocumentName");
            //
            // migrationBuilder.RenameColumn(
            //     name: "deleted_at",
            //     table: "marriage_record_archives",
            //     newName: "DeletedAt");
            //
            // migrationBuilder.RenameColumn(
            //     name: "marriage_record_archive_id",
            //     table: "marriage_record_archives",
            //     newName: "MarriageRecordArchiveId");
            //
            // migrationBuilder.RenameColumn(
            //     name: "name",
            //     table: "document_archives",
            //     newName: "Name");
            //
            // migrationBuilder.RenameColumn(
            //     name: "category",
            //     table: "document_archives",
            //     newName: "Category");
            //
            // migrationBuilder.RenameColumn(
            //     name: "import_date",
            //     table: "document_archives",
            //     newName: "ImportDate");
            //
            // migrationBuilder.RenameColumn(
            //     name: "deleted_at",
            //     table: "document_archives",
            //     newName: "DeletedAt");
            //
            // migrationBuilder.RenameColumn(
            //     name: "document_archive_id",
            //     table: "document_archives",
            //     newName: "DocumentArchiveId");

            migrationBuilder.RenameIndex(
                name: "ix_death_records_registry_number",
                table: "death_records",
                newName: "IX_death_records_registry_number");

            migrationBuilder.RenameIndex(
                name: "ix_death_records_person_id",
                table: "death_records",
                newName: "IX_death_records_person_id");

            migrationBuilder.RenameIndex(
                name: "ix_death_records_document_id",
                table: "death_records",
                newName: "IX_death_records_document_id");

            // migrationBuilder.RenameColumn(
            //     name: "registry_number",
            //     table: "death_record_archives",
            //     newName: "RegistryNumber");
            //
            // migrationBuilder.RenameColumn(
            //     name: "registry_date",
            //     table: "death_record_archives",
            //     newName: "RegistryDate");
            //
            // migrationBuilder.RenameColumn(
            //     name: "person_pesel",
            //     table: "death_record_archives",
            //     newName: "PersonPesel");
            //
            // migrationBuilder.RenameColumn(
            //     name: "document_name",
            //     table: "death_record_archives",
            //     newName: "DocumentName");
            //
            // migrationBuilder.RenameColumn(
            //     name: "deleted_at",
            //     table: "death_record_archives",
            //     newName: "DeletedAt");
            //
            // migrationBuilder.RenameColumn(
            //     name: "death_place",
            //     table: "death_record_archives",
            //     newName: "DeathPlace");
            //
            // migrationBuilder.RenameColumn(
            //     name: "death_date",
            //     table: "death_record_archives",
            //     newName: "DeathDate");
            //
            // migrationBuilder.RenameColumn(
            //     name: "cause_of_death",
            //     table: "death_record_archives",
            //     newName: "CauseOfDeath");
            //
            // migrationBuilder.RenameColumn(
            //     name: "death_record_archive_id",
            //     table: "death_record_archives",
            //     newName: "DeathRecordArchiveId");

            migrationBuilder.RenameIndex(
                name: "ix_birth_records_registry_number",
                table: "birth_records",
                newName: "IX_birth_records_registry_number");

            migrationBuilder.RenameIndex(
                name: "ix_birth_records_person_id",
                table: "birth_records",
                newName: "IX_birth_records_person_id");

            migrationBuilder.RenameIndex(
                name: "ix_birth_records_mother_id",
                table: "birth_records",
                newName: "IX_birth_records_mother_id");

            migrationBuilder.RenameIndex(
                name: "ix_birth_records_father_id",
                table: "birth_records",
                newName: "IX_birth_records_father_id");

            migrationBuilder.RenameIndex(
                name: "ix_birth_records_document_id",
                table: "birth_records",
                newName: "IX_birth_records_document_id");

            // migrationBuilder.RenameColumn(
            //     name: "registry_number",
            //     table: "birth_record_archives",
            //     newName: "RegistryNumber");
            //
            // migrationBuilder.RenameColumn(
            //     name: "registry_date",
            //     table: "birth_record_archives",
            //     newName: "RegistryDate");
            //
            // migrationBuilder.RenameColumn(
            //     name: "mother_pesel",
            //     table: "birth_record_archives",
            //     newName: "MotherPesel");
            //
            // migrationBuilder.RenameColumn(
            //     name: "father_pesel",
            //     table: "birth_record_archives",
            //     newName: "FatherPesel");
            //
            // migrationBuilder.RenameColumn(
            //     name: "document_name",
            //     table: "birth_record_archives",
            //     newName: "DocumentName");
            //
            // migrationBuilder.RenameColumn(
            //     name: "deleted_at",
            //     table: "birth_record_archives",
            //     newName: "DeletedAt");
            //
            // migrationBuilder.RenameColumn(
            //     name: "born_person_pesel",
            //     table: "birth_record_archives",
            //     newName: "BornPersonPesel");
            //
            // migrationBuilder.RenameColumn(
            //     name: "birth_place",
            //     table: "birth_record_archives",
            //     newName: "BirthPlace");
            //
            // migrationBuilder.RenameColumn(
            //     name: "birth_date",
            //     table: "birth_record_archives",
            //     newName: "BirthDate");
            //
            // migrationBuilder.RenameColumn(
            //     name: "birth_record_archive_id",
            //     table: "birth_record_archives",
            //     newName: "BirthRecordArchiveId");

            migrationBuilder.RenameIndex(
                name: "ix_addresses_document_id",
                table: "addresses",
                newName: "IX_addresses_document_id");

            // migrationBuilder.RenameColumn(
            //     name: "street",
            //     table: "address_archives",
            //     newName: "Street");
            //
            // migrationBuilder.RenameColumn(
            //     name: "country",
            //     table: "address_archives",
            //     newName: "Country");
            //
            // migrationBuilder.RenameColumn(
            //     name: "city",
            //     table: "address_archives",
            //     newName: "City");
            //
            // migrationBuilder.RenameColumn(
            //     name: "postal_code",
            //     table: "address_archives",
            //     newName: "PostalCode");
            //
            // migrationBuilder.RenameColumn(
            //     name: "house_number",
            //     table: "address_archives",
            //     newName: "HouseNumber");
            //
            // migrationBuilder.RenameColumn(
            //     name: "document_name",
            //     table: "address_archives",
            //     newName: "DocumentName");
            //
            // migrationBuilder.RenameColumn(
            //     name: "deleted_at",
            //     table: "address_archives",
            //     newName: "DeletedAt");
            //
            // migrationBuilder.RenameColumn(
            //     name: "apartment_number",
            //     table: "address_archives",
            //     newName: "ApartmentNumber");
            //
            // migrationBuilder.RenameColumn(
            //     name: "address_archive_id",
            //     table: "address_archives",
            //     newName: "AddressArchiveId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_persons",
                table: "persons",
                column: "person_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_person_archives",
                table: "person_archives",
                column: "PersonArchiveId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_marriage_records",
                table: "marriage_records",
                column: "marriage_record_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_marriage_record_archives",
                table: "marriage_record_archives",
                column: "MarriageRecordArchiveId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_documents",
                table: "documents",
                column: "document_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_document_archives",
                table: "document_archives",
                column: "DocumentArchiveId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_death_records",
                table: "death_records",
                column: "death_record_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_death_record_archives",
                table: "death_record_archives",
                column: "DeathRecordArchiveId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_birth_records",
                table: "birth_records",
                column: "birth_record_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_birth_record_archives",
                table: "birth_record_archives",
                column: "BirthRecordArchiveId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_addresses",
                table: "addresses",
                column: "address_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_address_archives",
                table: "address_archives",
                column: "AddressArchiveId");

            migrationBuilder.AddForeignKey(
                name: "FK_addresses_documents_document_id",
                table: "addresses",
                column: "document_id",
                principalTable: "documents",
                principalColumn: "document_id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_birth_records_documents_document_id",
                table: "birth_records",
                column: "document_id",
                principalTable: "documents",
                principalColumn: "document_id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_birth_records_persons_father_id",
                table: "birth_records",
                column: "father_id",
                principalTable: "persons",
                principalColumn: "person_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_birth_records_persons_mother_id",
                table: "birth_records",
                column: "mother_id",
                principalTable: "persons",
                principalColumn: "person_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_birth_records_persons_person_id",
                table: "birth_records",
                column: "person_id",
                principalTable: "persons",
                principalColumn: "person_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_death_records_documents_document_id",
                table: "death_records",
                column: "document_id",
                principalTable: "documents",
                principalColumn: "document_id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_death_records_persons_person_id",
                table: "death_records",
                column: "person_id",
                principalTable: "persons",
                principalColumn: "person_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_marriage_records_documents_document_id",
                table: "marriage_records",
                column: "document_id",
                principalTable: "documents",
                principalColumn: "document_id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_marriage_records_persons_spouse1_id",
                table: "marriage_records",
                column: "spouse1_id",
                principalTable: "persons",
                principalColumn: "person_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_marriage_records_persons_spouse2_id",
                table: "marriage_records",
                column: "spouse2_id",
                principalTable: "persons",
                principalColumn: "person_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_persons_addresses_address_id",
                table: "persons",
                column: "address_id",
                principalTable: "addresses",
                principalColumn: "address_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_persons_documents_document_id",
                table: "persons",
                column: "document_id",
                principalTable: "documents",
                principalColumn: "document_id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}

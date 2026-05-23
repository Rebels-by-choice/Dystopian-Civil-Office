using System;
using Dystopian_Civil_Office.DataSource;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Dystopian_Civil_Office.Migrations
{
    /// <inheritdoc />
    public partial class InitialPipeline : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Cleanup tables between engaging
            ExecuteEmbeddedSql(migrationBuilder, "Dystopian_Civil_Office.InitDb.Sql.Clean.drop_cascade_tables.sql");
            ExecuteEmbeddedSql(migrationBuilder, "Dystopian_Civil_Office.InitDb.Sql.Clean.drop_subobjects.sql");
            
            migrationBuilder.CreateTable(
                name: "documents",
                columns: table => new
                {
                    document_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    import_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_documents", x => x.document_id);
                });

            migrationBuilder.CreateTable(
                name: "addresses",
                columns: table => new
                {
                    address_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    city = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    street = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    house_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    apartment_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    postal_code = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    country = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    document_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_addresses", x => x.address_id);
                    table.ForeignKey(
                        name: "FK_addresses_documents_document_id",
                        column: x => x.document_id,
                        principalTable: "documents",
                        principalColumn: "document_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "persons",
                columns: table => new
                {
                    person_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pesel = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    first_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    middle_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    last_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    gender = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    birth_date = table.Column<DateOnly>(type: "date", nullable: false),
                    birth_place = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    address_id = table.Column<int>(type: "integer", nullable: false),
                    document_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_persons", x => x.person_id);
                    table.CheckConstraint("ck_persons_gender", "\"gender\" IN ('Male', 'Female', 'Other')");
                    table.ForeignKey(
                        name: "FK_persons_addresses_address_id",
                        column: x => x.address_id,
                        principalTable: "addresses",
                        principalColumn: "address_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_persons_documents_document_id",
                        column: x => x.document_id,
                        principalTable: "documents",
                        principalColumn: "document_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "birth_records",
                columns: table => new
                {
                    birth_record_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    registry_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    person_id = table.Column<int>(type: "integer", nullable: false),
                    mother_id = table.Column<int>(type: "integer", nullable: true),
                    father_id = table.Column<int>(type: "integer", nullable: true),
                    registry_date = table.Column<DateOnly>(type: "date", nullable: false),
                    document_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_birth_records", x => x.birth_record_id);
                    table.ForeignKey(
                        name: "FK_birth_records_documents_document_id",
                        column: x => x.document_id,
                        principalTable: "documents",
                        principalColumn: "document_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_birth_records_persons_father_id",
                        column: x => x.father_id,
                        principalTable: "persons",
                        principalColumn: "person_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_birth_records_persons_mother_id",
                        column: x => x.mother_id,
                        principalTable: "persons",
                        principalColumn: "person_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_birth_records_persons_person_id",
                        column: x => x.person_id,
                        principalTable: "persons",
                        principalColumn: "person_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "death_records",
                columns: table => new
                {
                    death_record_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    registry_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    person_id = table.Column<int>(type: "integer", nullable: false),
                    death_date = table.Column<DateOnly>(type: "date", nullable: false),
                    death_place = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    registry_date = table.Column<DateOnly>(type: "date", nullable: false),
                    cause_of_death = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    document_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_death_records", x => x.death_record_id);
                    table.ForeignKey(
                        name: "FK_death_records_documents_document_id",
                        column: x => x.document_id,
                        principalTable: "documents",
                        principalColumn: "document_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_death_records_persons_person_id",
                        column: x => x.person_id,
                        principalTable: "persons",
                        principalColumn: "person_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "marriage_records",
                columns: table => new
                {
                    marriage_record_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    registry_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    spouse1_id = table.Column<int>(type: "integer", nullable: false),
                    spouse2_id = table.Column<int>(type: "integer", nullable: false),
                    marriage_date = table.Column<DateOnly>(type: "date", nullable: false),
                    marriage_place = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    registry_date = table.Column<DateOnly>(type: "date", nullable: false),
                    document_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_marriage_records", x => x.marriage_record_id);
                    table.CheckConstraint("ck_marriage_records_spouses_different", "\"spouse1_id\" <> \"spouse2_id\"");
                    table.ForeignKey(
                        name: "FK_marriage_records_documents_document_id",
                        column: x => x.document_id,
                        principalTable: "documents",
                        principalColumn: "document_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_marriage_records_persons_spouse1_id",
                        column: x => x.spouse1_id,
                        principalTable: "persons",
                        principalColumn: "person_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_marriage_records_persons_spouse2_id",
                        column: x => x.spouse2_id,
                        principalTable: "persons",
                        principalColumn: "person_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_addresses_document_id",
                table: "addresses",
                column: "document_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_birth_records_document_id",
                table: "birth_records",
                column: "document_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_birth_records_father_id",
                table: "birth_records",
                column: "father_id");

            migrationBuilder.CreateIndex(
                name: "IX_birth_records_mother_id",
                table: "birth_records",
                column: "mother_id");

            migrationBuilder.CreateIndex(
                name: "IX_birth_records_person_id",
                table: "birth_records",
                column: "person_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_birth_records_registry_number",
                table: "birth_records",
                column: "registry_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_death_records_document_id",
                table: "death_records",
                column: "document_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_death_records_person_id",
                table: "death_records",
                column: "person_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_death_records_registry_number",
                table: "death_records",
                column: "registry_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_marriage_records_document_id",
                table: "marriage_records",
                column: "document_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_marriage_records_registry_number",
                table: "marriage_records",
                column: "registry_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_marriage_records_spouse1_id",
                table: "marriage_records",
                column: "spouse1_id");

            migrationBuilder.CreateIndex(
                name: "IX_marriage_records_spouse2_id",
                table: "marriage_records",
                column: "spouse2_id");

            migrationBuilder.CreateIndex(
                name: "IX_persons_address_id",
                table: "persons",
                column: "address_id");

            migrationBuilder.CreateIndex(
                name: "IX_persons_document_id",
                table: "persons",
                column: "document_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_persons_pesel",
                table: "persons",
                column: "pesel",
                unique: true);
            
            migrationBuilder.CreateTable(
                name: "address_archives",
                columns: table => new
                {
                    AddressArchiveId = table.Column<int>(name: "address_archive_id", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", Npgsql.EntityFrameworkCore.PostgreSQL.Metadata.NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    City = table.Column<string>(name: "city", type: "character varying(100)", maxLength: 100, nullable: false),
                    Street = table.Column<string>(name: "street", type: "character varying(150)", maxLength: 150, nullable: false),
                    HouseNumber = table.Column<string>(name: "house_number", type: "character varying(20)", maxLength: 20, nullable: false),
                    ApartmentNumber = table.Column<string>(name: "apartment_number", type: "character varying(20)", maxLength: 20, nullable: true),
                    PostalCode = table.Column<string>(name: "postal_code", type: "character varying(20)", maxLength: 20, nullable: false),
                    Country = table.Column<string>(name: "country", type: "character varying(100)", maxLength: 100, nullable: false),
                    DocumentName = table.Column<string>(name: "document_name", type: "character varying(255)", maxLength: 255, nullable: true),
                    DeletedAt = table.Column<DateTime>(name: "deleted_at", type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_address_archives", x => x.AddressArchiveId);
                });

            migrationBuilder.CreateTable(
                name: "birth_record_archives",
                columns: table => new
                {
                    BirthRecordArchiveId = table.Column<int>(name: "birth_record_archive_id", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", Npgsql.EntityFrameworkCore.PostgreSQL.Metadata.NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RegistryNumber = table.Column<string>(name: "registry_number", type: "character varying(50)", maxLength: 50, nullable: false),
                    RegistryDate = table.Column<DateOnly>(name: "registry_date", type: "date", nullable: false),
                    BornPersonPesel = table.Column<string>(name: "born_person_pesel", type: "character varying(11)", maxLength: 11, nullable: false),
                    MotherPesel = table.Column<string>(name: "mother_pesel", type: "character varying(11)", maxLength: 11, nullable: true),
                    FatherPesel = table.Column<string>(name: "father_pesel", type: "character varying(11)", maxLength: 11, nullable: true),
                    BirthDate = table.Column<DateOnly>(name: "birth_date", type: "date", nullable: false),
                    BirthPlace = table.Column<string>(name: "birth_place", type: "character varying(200)", maxLength: 200, nullable: false),
                    DocumentName = table.Column<string>(name: "document_name", type: "character varying(255)", maxLength: 255, nullable: true),
                    DeletedAt = table.Column<DateTime>(name: "deleted_at", type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_birth_record_archives", x => x.BirthRecordArchiveId);
                });

            migrationBuilder.CreateTable(
                name: "death_record_archives",
                columns: table => new
                {
                    DeathRecordArchiveId = table.Column<int>(name: "death_record_archive_id", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", Npgsql.EntityFrameworkCore.PostgreSQL.Metadata.NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RegistryNumber = table.Column<string>(name: "registry_number", type: "character varying(50)", maxLength: 50, nullable: false),
                    RegistryDate = table.Column<DateOnly>(name: "registry_date", type: "date", nullable: false),
                    PersonPesel = table.Column<string>(name: "person_pesel", type: "character varying(11)", maxLength: 11, nullable: false),
                    DeathDate = table.Column<DateOnly>(name: "death_date", type: "date", nullable: false),
                    DeathPlace = table.Column<string>(name: "death_place", type: "character varying(200)", maxLength: 200, nullable: false),
                    CauseOfDeath = table.Column<string>(name: "cause_of_death", type: "character varying(200)", maxLength: 200, nullable: false),
                    DocumentName = table.Column<string>(name: "document_name", type: "character varying(255)", maxLength: 255, nullable: true),
                    DeletedAt = table.Column<DateTime>(name: "deleted_at", type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_death_record_archives", x => x.DeathRecordArchiveId);
                });

            migrationBuilder.CreateTable(
                name: "document_archives",
                columns: table => new
                {
                    DocumentArchiveId = table.Column<int>(name: "document_archive_id", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", Npgsql.EntityFrameworkCore.PostgreSQL.Metadata.NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(name: "name", type: "character varying(255)", maxLength: 255, nullable: false),
                    Category = table.Column<string>(name: "category", type: "character varying(100)", maxLength: 100, nullable: false),
                    ImportDate = table.Column<DateTime>(name: "import_date", type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(name: "deleted_at", type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_document_archives", x => x.DocumentArchiveId);
                });

            migrationBuilder.CreateTable(
                name: "marriage_record_archives",
                columns: table => new
                {
                    MarriageRecordArchiveId = table.Column<int>(name: "marriage_record_archive_id", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", Npgsql.EntityFrameworkCore.PostgreSQL.Metadata.NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RegistryNumber = table.Column<string>(name: "registry_number", type: "character varying(50)", maxLength: 50, nullable: false),
                    RegistryDate = table.Column<DateOnly>(name: "registry_date", type: "date", nullable: false),
                    Spouse1Pesel = table.Column<string>(name: "spouse1_pesel", type: "character varying(11)", maxLength: 11, nullable: false),
                    Spouse2Pesel = table.Column<string>(name: "spouse2_pesel", type: "character varying(11)", maxLength: 11, nullable: false),
                    MarriageDate = table.Column<DateOnly>(name: "marriage_date", type: "date", nullable: false),
                    MarriagePlace = table.Column<string>(name: "marriage_place", type: "character varying(200)", maxLength: 200, nullable: false),
                    DocumentName = table.Column<string>(name: "document_name", type: "character varying(255)", maxLength: 255, nullable: true),
                    DeletedAt = table.Column<DateTime>(name: "deleted_at", type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_marriage_record_archives", x => x.MarriageRecordArchiveId);
                });

            migrationBuilder.CreateTable(
                name: "person_archives",
                columns: table => new
                {
                    PersonArchiveId = table.Column<int>(name: "person_archive_id", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", Npgsql.EntityFrameworkCore.PostgreSQL.Metadata.NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonPesel = table.Column<string>(name: "person_pesel", type: "character varying(11)", maxLength: 11, nullable: false),
                    FirstName = table.Column<string>(name: "first_name", type: "character varying(100)", maxLength: 100, nullable: false),
                    MiddleName = table.Column<string>(name: "middle_name", type: "character varying(100)", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(name: "last_name", type: "character varying(100)", maxLength: 100, nullable: false),
                    Gender = table.Column<string>(name: "gender", type: "character varying(20)", maxLength: 20, nullable: false),
                    BirthDate = table.Column<DateOnly>(name: "birth_date", type: "date", nullable: false),
                    BirthPlace = table.Column<string>(name: "birth_place", type: "character varying(200)", maxLength: 200, nullable: false),
                    DocumentName = table.Column<string>(name: "document_name", type: "character varying(255)", maxLength: 255, nullable: true),
                    DeletedAt = table.Column<DateTime>(name: "deleted_at", type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_person_archives", x => x.PersonArchiveId);
                });
            
            /*
             * SQL Section
             * Clean, Seed, Functions, Procedures, Triggers
             */
            ExecuteEmbeddedSql(migrationBuilder, "Dystopian_Civil_Office.InitDb.Sql.Seed.seed_mocks.init.sql");

            ExecuteEmbeddedSql(migrationBuilder, "Dystopian_Civil_Office.InitDb.Sql.Functions.validate_data.defs.sql");
            ExecuteEmbeddedSql(migrationBuilder, "Dystopian_Civil_Office.InitDb.Sql.Functions.archive_data.defs.sql");

            ExecuteEmbeddedSql(migrationBuilder, "Dystopian_Civil_Office.InitDb.Sql.Procedures.addresses.procedures.init.sql");
            ExecuteEmbeddedSql(migrationBuilder, "Dystopian_Civil_Office.InitDb.Sql.Procedures.birth_records.procedures.init.sql");
            ExecuteEmbeddedSql(migrationBuilder, "Dystopian_Civil_Office.InitDb.Sql.Procedures.death_records.procedures.init.sql");
            ExecuteEmbeddedSql(migrationBuilder, "Dystopian_Civil_Office.InitDb.Sql.Procedures.documents.procedures.init.sql");
            ExecuteEmbeddedSql(migrationBuilder, "Dystopian_Civil_Office.InitDb.Sql.Procedures.marriage_records.procedures.init.sql");
            ExecuteEmbeddedSql(migrationBuilder, "Dystopian_Civil_Office.InitDb.Sql.Procedures.persons.procedures.init.sql");

            ExecuteEmbeddedSql(migrationBuilder, "Dystopian_Civil_Office.InitDb.Sql.Triggers.archive_data.triggers.sql");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "birth_records");
            migrationBuilder.DropTable(name: "death_records");
            migrationBuilder.DropTable(name: "marriage_records");
            migrationBuilder.DropTable(name: "persons");
            migrationBuilder.DropTable(name: "addresses");
            migrationBuilder.DropTable(name: "documents");
            
            migrationBuilder.DropTable(name: "document_archives");
            migrationBuilder.DropTable(name: "person_archives");
            migrationBuilder.DropTable(name: "birth_record_archives");
            migrationBuilder.DropTable(name: "death_record_archives");
            migrationBuilder.DropTable(name: "marriage_record_archives");
            migrationBuilder.DropTable(name: "address_archives");
        }
        
        // Method for reading .sql file ad executing it
        private static void ExecuteEmbeddedSql(MigrationBuilder migrationBuilder, string resourceName)
        {
            var assembly = typeof(ApplicationDbContext).Assembly;

            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream is null)
                throw new InvalidOperationException($"Embedded SQL resource not found: {resourceName}");

            using var reader = new StreamReader(stream);
            var sql = reader.ReadToEnd();

            migrationBuilder.Sql(sql);
        }
    }
}
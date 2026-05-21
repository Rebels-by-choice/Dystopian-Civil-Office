﻿using System;
using Dystopian_Civil_Office.DataSource;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Dystopian_Civil_Office.Migrations
{
    /// <inheritdoc />
    public partial class InitialObjectsAndMocks_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Cleanup tables between engaging
            ExecuteEmbeddedSql(migrationBuilder, "Dystopian_Civil_Office.InitDb.Sql.dropCascadeTables.sql");
            
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
            /*
             * SQL Section
             * Mocks, Functions, Procedures and Triggers
             */
            ExecuteEmbeddedSql(migrationBuilder, "Dystopian_Civil_Office.InitDb.Sql.clearMocks_and_subobjects.sql");
            ExecuteEmbeddedSql(migrationBuilder, "Dystopian_Civil_Office.InitDb.Sql.mocks.init.sql");
            ExecuteEmbeddedSql(migrationBuilder, "Dystopian_Civil_Office.InitDb.Sql.functions.init.sql");
            ExecuteEmbeddedSql(migrationBuilder, "Dystopian_Civil_Office.InitDb.Sql.procedures.init.sql");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "birth_records");

            migrationBuilder.DropTable(
                name: "death_records");

            migrationBuilder.DropTable(
                name: "marriage_records");

            migrationBuilder.DropTable(
                name: "persons");

            migrationBuilder.DropTable(
                name: "addresses");

            migrationBuilder.DropTable(
                name: "documents");
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
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dystopian_Civil_Office.Migrations
{
    /// <inheritdoc />
    public partial class AddRegistryNumberToAddressArchive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "registry_number",
                table: "address_archives",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "registry_number",
                table: "address_archives");
        }
    }
}

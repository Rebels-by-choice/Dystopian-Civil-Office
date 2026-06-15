using Dystopian_Civil_Office.DataSource;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dystopian_Civil_Office.Migrations;

public static class MigrationHelpers
{
    public static void ExecuteEmbeddedSql(MigrationBuilder migrationBuilder, string resourceName)
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
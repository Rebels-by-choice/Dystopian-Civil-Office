using Dystopian_Civil_Office.Exceptions;

namespace Dystopian_Civil_Office.Services.Validation;

public sealed class QueryValidationService : IQueryValidationService
{
    public void ValidateCategory(string? category)
    {
        if (string.IsNullOrWhiteSpace(category))
        {
            throw new DatasetNotFoundException(
                title: "Dataset not found",
                description: "Wrong filter parameter");
        }
    }
}
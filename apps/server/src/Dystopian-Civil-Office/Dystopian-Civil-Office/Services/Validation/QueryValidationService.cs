using Dystopian_Civil_Office.Exceptions;

namespace Dystopian_Civil_Office.Services.Validation;

public sealed class QueryValidationService : IQueryValidationService
{
    public void ValidateCategory(string? category)
    {
        var allowedCategories = new[]
        {
            "BirthRecord", "Person", "Address", "DeathRecord", "MarriageRecord"
        };

        if (string.IsNullOrWhiteSpace(category) || !allowedCategories.Contains(category))
        {
            throw new DatasetNotFoundException(
                title: "Invalid filter parameter",
                description: "Category has an invalid value.");
        }
    }

    public void ValidateGender(string? gender)
    {
        var allowedGenders = new[]
        {
            "Male", "Female", "Other"
        };
        
        if (string.IsNullOrWhiteSpace(gender) || !allowedGenders.Contains(gender))
        {
            throw new DatasetNotFoundException(
                title: "Invalid filter parameter", 
                description: "Gender has an invalid value.");
        }
    }
}
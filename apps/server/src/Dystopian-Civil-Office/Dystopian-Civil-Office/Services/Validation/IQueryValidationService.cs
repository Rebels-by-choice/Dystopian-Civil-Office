namespace Dystopian_Civil_Office.Services.Validation;

public interface IQueryValidationService
{
    void ValidateCategory(string? category);
    void ValidateGender(string? gender);
}
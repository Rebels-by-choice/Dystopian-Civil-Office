namespace Dystopian_Civil_Office.Exceptions;

public sealed class DatasetNotFoundException : Exception
{
    public int StatusCode { get; }
    public string Title { get; }
    public string Description { get; }

    public DatasetNotFoundException(
        string title = "Dataset not found",
        string description = "Wrong filter parameter",
        int statusCode = StatusCodes.Status404NotFound)
        : base(description)
    {
        Title = title;
        Description = description;
        StatusCode = statusCode;
    }
}
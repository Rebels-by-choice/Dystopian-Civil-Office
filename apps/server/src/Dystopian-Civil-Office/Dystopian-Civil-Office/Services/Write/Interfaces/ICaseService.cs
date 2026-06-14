namespace Dystopian_Civil_Office.Services.Write.Interfaces;

public interface ICaseService
{
    Task CreateAsync();
    Task UpdateStatusAsync();
}
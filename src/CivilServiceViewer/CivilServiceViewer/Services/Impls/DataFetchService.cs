using CivilServiceViewer.Models;
using Microsoft.EntityFrameworkCore;

namespace CivilServiceViewer.Services
{
    // This class is responsible for fetching data from database and returning it as IEnumerable<T>.
    public class DataFetchService : IDataOperationsService
    {
        private readonly AppDbContext _db;

        public DataFetchService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<T>> FetchDataAsync<T>() where T : class
        {
            return await _db.Set<T>()
                            .AsNoTracking()
                            .ToListAsync();
        }
    }
}

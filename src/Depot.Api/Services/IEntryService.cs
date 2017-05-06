using System.Collections.Generic;
using System.Threading.Tasks;
using Depot.Api.Models;

namespace Depot.Api.Services
{
    public interface IEntryService
    {
        Task<IEnumerable<Entry>> BrowseAsync();
        Task<Entry> GetAsync(string key);
        Task CreateAsync(string key, string value);
        Task DeleteAsync(string key);
    }
}
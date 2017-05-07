using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Depot.Api.Models;
using Depot.Api.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Depot.Api.Services
{
    public class EntryService : IEntryService
    {
        private static readonly ISet<Entry> _entries = new HashSet<Entry>();

        public EntryService(IOptions<AppOptions> appOptions, 
            ILogger<EntryService> logger)
        {
            if(appOptions.Value.SeedData && !_entries.Any())
            {
                _entries.Add(new Entry("k1", "entry1"));
                _entries.Add(new Entry("k2", "entry2"));
                _entries.Add(new Entry("k3", "entry3"));
                logger.LogInformation("Data was initialized");
            }
        }

        public async Task<IEnumerable<Entry>> BrowseAsync()
            => await Task.FromResult(_entries);

        public async Task<Entry> GetAsync(string key)
            => await Task.FromResult(_entries.SingleOrDefault(x => x.Key == key));

        public async Task CreateAsync(string key, string value)
        {
            var entry = await GetAsync(key);
            if(entry != null)
            {
                throw new Exception($"Entry '{key}' already exists.");
            }
            _entries.Add(new Entry(key, value));
        }

        public async Task DeleteAsync(string key)
        {
            var entry = await GetAsync(key);
            if(entry == null)
            {
                throw new Exception($"Entry '{key}' was not found.");
            }
            _entries.Remove(entry);
        }
    }
}
using System.Linq;
using System.Threading.Tasks;
using Depot.Api.Commands;
using Depot.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Depot.Api.Controllers
{
    [Route("[controller]")]
    public class EntriesController : Controller
    {
        private readonly IEntryService _entryService;

        public EntriesController(IEntryService entryService)
        {
            _entryService = entryService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var entries = await _entryService.BrowseAsync();

            return Json(entries.Select(x => x.Key));
        }

        [HttpGet("{key}")]
        public async Task<IActionResult> Get(string key)
        {
            var entry = await _entryService.GetAsync(key);
            if(entry == null)
            {
                return NotFound();
            }

            return Json(entry.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateEntry command)
        {
            await _entryService.CreateAsync(command.Key, command.Value);

            return Created($"entries/{command.Key}", null);
        }

        [HttpDelete("{key}")]
        public async Task<IActionResult> Delete(string key)
        {
            await _entryService.DeleteAsync(key);

            return NoContent();
        }                        
    }
}
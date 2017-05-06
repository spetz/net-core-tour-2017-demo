using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Depot.Api.Commands;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Xunit;

namespace Depot.Api.Tests
{
    public class EntriesControllerTests
    {
        protected readonly TestServer Server;
        protected readonly HttpClient Client;

        public EntriesControllerTests()
        {
            Server = new TestServer(new WebHostBuilder()
                          .UseStartup<Startup>());
            Client = Server.CreateClient();
        }

        [Fact]
        public async Task given_valid_key_entry_should_exist()
        {
            var key = "k1";
            var value = "entry1";
            var entry = await GetEntryValueAsync(key);
            entry.ShouldBeEquivalentTo(value);
        }

        [Fact]
        public async Task given_invalid_key_entry_should_not_exist()
        {
            var key = "invalid";
            var response = await Client.GetAsync($"entries/{key}");
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task given_unique_key_entry_should_be_created()
        {
            var command = new CreateEntry 
            {
                Key = "test",
                Value = "test-entry"
            };
            var payload = GetPayload(command);
            var response = await Client.PostAsync("entries", payload);
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.Created);
            response.Headers.Location.ToString().ShouldBeEquivalentTo($"entries/{command.Key}");

            var entryValue = await GetEntryValueAsync(command.Key);
            entryValue.ShouldBeEquivalentTo(command.Value);
        }

        private async Task<string> GetEntryValueAsync(string key)
        {
            var response = await Client.GetAsync($"entries/{key}");
            var responseString = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<string>(responseString);
        }

        protected static StringContent GetPayload(object data)
        {
            var json = JsonConvert.SerializeObject(data);

            return new StringContent(json, Encoding.UTF8, "application/json");
        }             
    }
}
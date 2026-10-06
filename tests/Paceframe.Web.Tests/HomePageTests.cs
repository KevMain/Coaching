using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Run_coaching;

namespace Paceframe.Web.Tests
{
    public class HomePageTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public HomePageTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Get_Home_ReturnsSuccessAndContainsHeadline()
        {
            var client = _factory.CreateClient();
            var res = await client.GetAsync("/");
            res.EnsureSuccessStatusCode();
            var content = await res.Content.ReadAsStringAsync();
            Assert.Contains("Train smarter. Run stronger.", content);
        }
    }
}

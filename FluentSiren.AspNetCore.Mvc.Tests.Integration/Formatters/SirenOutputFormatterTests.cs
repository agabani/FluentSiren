using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentSiren.AspNetCore.Mvc.Formatters;
using FluentSiren.Builders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace FluentSiren.AspNetCore.Mvc.Tests.Integration.Formatters
{
    public class SirenOutputFormatterTests : IDisposable
    {
        public SirenOutputFormatterTests()
        {
            _server = new TestServer(new WebHostBuilder().UseStartup<StartUp>());
            _client = _server.CreateClient();
        }

        public void Dispose()
        {
            _client?.Dispose();
            _server?.Dispose();
        }

        private readonly HttpClient _client;
        private readonly TestServer _server;

        [Fact]
        public async Task Test()
        {
            const string sirenMediaType = "application/vnd.siren+json";

            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.example.com/api/Value");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(sirenMediaType));

            using (var response = await _client.SendAsync(request))
            {
                Assert.Contains(sirenMediaType, response.Content.Headers.ContentType.MediaType);
                Assert.Equal("{\"class\":[\"class\"]}", await response.Content.ReadAsStringAsync());
            }
        }
    }

    public class StartUp
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => { options.OutputFormatters.Add(new SirenOutputFormatter()); });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMvc(route => { route.MapRoute("default", "api/{controller=Value}/{action=Get}/{id?}"); });
        }
    }

    public class ValueController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new EntityBuilder().WithClass("class").Build());
        }
    }
}
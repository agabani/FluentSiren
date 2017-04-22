using System;
using System.Net.Http;
using FluentSiren.Builders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;

namespace FluentSiren.AspNetCore.Mvc.Tests.Integration.Formatters
{
    public abstract class SirenOutputFormatterTestsBase<TStartup> : IDisposable where TStartup : class
    {
        protected readonly HttpClient Client;
        protected readonly TestServer Server;

        protected SirenOutputFormatterTestsBase()
        {
            Server = new TestServer(new WebHostBuilder().UseStartup<TStartup>());
            Client = Server.CreateClient();
        }

        public void Dispose()
        {
            Client?.Dispose();
            Server?.Dispose();
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
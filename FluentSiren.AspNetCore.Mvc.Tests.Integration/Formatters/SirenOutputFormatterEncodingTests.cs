using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FluentSiren.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace FluentSiren.AspNetCore.Mvc.Tests.Integration.Formatters
{
    public class SirenOutputFormatterEncodingTests : SirenOutputFormatterTestsBase<StartUpEncoding>
    {
        [Fact]
        public async Task Test()
        {
            const string sirenMediaType = "application/vnd.siren+json";

            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.example.com/api/Value");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(sirenMediaType));

            using (var response = await Client.SendAsync(request))
            {
                Assert.Contains(sirenMediaType, response.Content.Headers.ContentType.MediaType);
                Assert.Equal("utf-16BE", response.Content.Headers.ContentType.CharSet);
                Assert.Equal("{\"class\":[\"class\"]}", await response.Content.ReadAsStringAsync());
            }
        }
    }

    public class StartUpEncoding
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.OutputFormatters.Add(new SirenOutputFormatter(Encoding.BigEndianUnicode));
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMvc(route => { route.MapRoute("default", "api/{controller=Value}/{action=Get}/{id?}"); });
        }
    }
}
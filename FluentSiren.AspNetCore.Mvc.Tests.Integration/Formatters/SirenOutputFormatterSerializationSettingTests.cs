using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentSiren.AspNetCore.Mvc.Formatters;
using FluentSiren.Builders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Xunit;

namespace FluentSiren.AspNetCore.Mvc.Tests.Integration.Formatters
{
    public class
        SirenOutputFormatterSerializationSettingTests : SirenOutputFormatterTestsBase<StartUpSerializationSetting>
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
                Assert.Equal("utf-8", response.Content.Headers.ContentType.CharSet);
                Assert.Equal(JsonConvert.SerializeObject(new EntityBuilder().WithClass("class").Build(),
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        NullValueHandling = NullValueHandling.Include
                    }), await response.Content.ReadAsStringAsync());
            }
        }
    }

    public class StartUpSerializationSetting
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.OutputFormatters.Add(new SirenOutputFormatter(new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Include
                }));
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMvc(route => { route.MapRoute("default", "api/{controller=Value}/{action=Get}/{id?}"); });
        }
    }
}
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;
using FluentSiren.AspNet.WebApi.Formatting;
using FluentSiren.Builders;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;

namespace FluentSiren.AspNet.WebApi.Tests.Integration.Formatting
{
    [TestFixture]
    public class SirenMediaFormatterTests
    {
        [SetUp]
        public void SetUp()
        {
            _configuration = new HttpConfiguration();

            _configuration.Routes.MapHttpRoute("Default", "api/{controller}/{action}/{id}", new {id = RouteParameter.Optional});
            _configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            _httpServer = new HttpServer(_configuration);
            _httpClient = new HttpClient(_httpServer);
        }

        [TearDown]
        public void TearDown()
        {
            if (_httpClient != null)
            {
                _httpClient.Dispose();
                _httpClient = null;
            }

            if (_httpServer != null)
            {
                _httpServer.Dispose();
                _httpServer = null;
            }
        }

        private HttpServer _httpServer;
        private HttpClient _httpClient;
        private HttpConfiguration _configuration;

        public class ValueController : ApiController
        {
            [HttpGet]
            public IHttpActionResult Get()
            {
                return Ok(new EntityBuilder().WithClass("value class").Build());
            }
        }

        [Test]
        public void Test()
        {
            const string sirenMediaType = "application/vnd.siren+json";

            _configuration.Formatters.Add(new SirenMediaFormatter());

            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.example.com/api/Value/Get");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(sirenMediaType));

            var expectedJson = JsonConvert.SerializeObject(new EntityBuilder().WithClass("value class").Build(), Newtonsoft.Json.Formatting.None, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            });

            using (var response = _httpClient.SendAsync(request).Result)
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response.Content.Headers.ContentType.ToString(), Is.EqualTo(sirenMediaType));
                Assert.That(response.Content.ReadAsStringAsync().Result, Is.EqualTo(expectedJson));
            }
        }

        [Test]
        public void Test_Encoding()
        {
            const string sirenMediaType = "application/vnd.siren+json";

            _configuration.Formatters.Add(new SirenMediaFormatter(Encoding.BigEndianUnicode));

            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.example.com/api/Value/Get");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(sirenMediaType));

            var expectedJson = JsonConvert.SerializeObject(new EntityBuilder().WithClass("value class").Build(), Newtonsoft.Json.Formatting.None, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                
            });

            using (var response = _httpClient.SendAsync(request).Result)
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response.Content.Headers.ContentType.ToString(), Is.EqualTo(sirenMediaType));

                var result = response.Content.ReadAsStringAsync().Result;
                var actualJson = Encoding.BigEndianUnicode.GetString(Encoding.UTF8.GetBytes(result));

                Assert.That(actualJson, Is.EqualTo(expectedJson));
            }
        }

        [Test]
        public void Test_Encoding_and_Serialization_Setting()
        {
            const string sirenMediaType = "application/vnd.siren+json";

            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };

            _configuration.Formatters.Add(new SirenMediaFormatter(jsonSerializerSettings, Encoding.BigEndianUnicode));

            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.example.com/api/Value/Get");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(sirenMediaType));

            var expectedJson = JsonConvert.SerializeObject(new EntityBuilder().WithClass("value class").Build(), Newtonsoft.Json.Formatting.None, jsonSerializerSettings);

            using (var response = _httpClient.SendAsync(request).Result)
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response.Content.Headers.ContentType.ToString(), Is.EqualTo(sirenMediaType));

                var result = response.Content.ReadAsStringAsync().Result;
                var actualJson = Encoding.BigEndianUnicode.GetString(Encoding.UTF8.GetBytes(result));

                Assert.That(actualJson, Is.EqualTo(expectedJson));
            }
        }

        [Test]
        public void Test_Serialization_Setting()
        {
            const string sirenMediaType = "application/vnd.siren+json";

            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Include
            };

            _configuration.Formatters.Add(new SirenMediaFormatter(jsonSerializerSettings));

            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.example.com/api/Value/Get");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(sirenMediaType));

            var expectedJson = JsonConvert.SerializeObject(new EntityBuilder().WithClass("value class").Build(), Newtonsoft.Json.Formatting.None, jsonSerializerSettings);

            using (var response = _httpClient.SendAsync(request).Result)
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response.Content.Headers.ContentType.ToString(), Is.EqualTo(sirenMediaType));
                Assert.That(response.Content.ReadAsStringAsync().Result, Is.EqualTo(expectedJson));
            }
        }
    }
}
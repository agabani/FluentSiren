using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentSiren.AspNetCore.Mvc.Formatters;
using FluentSiren.Builders;
using FluentSiren.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Xunit;

namespace FluentSiren.AspNetCore.Mvc.Tests.Unit.Formatters
{
    public class SirenOutputFormatterTests
    {
        public SirenOutputFormatterTests()
        {
            _sirenOutputFormatter = new SirenOutputFormatter();
        }

        private readonly SirenOutputFormatter _sirenOutputFormatter;

        [Fact]
        public void it_can_write_entity()
        {
            var canWriteResult = _sirenOutputFormatter.CanWriteResult(new OutputFormatterWriteContext(
                new DefaultHttpContext(),
                (stream, encoding) => TextWriter.Null,
                typeof(Entity),
                null));

            Assert.True(canWriteResult);
        }

        [Fact]
        public async Task it_can_write_to_stream()
        {
            using (var memoryStream = new MemoryStream())
            {
                var entity = new EntityBuilder().WithClass("class").Build();

                await _sirenOutputFormatter.WriteAsync(new OutputFormatterWriteContext(
                    new DefaultHttpContext {Response = {Body = memoryStream}},
                    (stream, encoding) => new HttpResponseStreamWriter(stream, Encoding.UTF8),
                    entity.GetType(),
                    entity));

                memoryStream.Seek(0, SeekOrigin.Begin);

                using (var streamReader = new StreamReader(memoryStream))
                {
                    Assert.Equal(JsonConvert.SerializeObject(entity,
                        new JsonSerializerSettings
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver(),
                            NullValueHandling = NullValueHandling.Ignore
                        }), streamReader.ReadToEnd());
                }
            }
        }

        [Fact]
        public async Task it_can_write_to_stream_with_cancellation_token()
        {
            using (var memoryStream = new MemoryStream())
            {
                var entity = new EntityBuilder().WithClass("class").Build();

                await _sirenOutputFormatter.WriteAsync(new OutputFormatterWriteContext(
                    new DefaultHttpContext {Response = {Body = memoryStream}, RequestAborted = CancellationToken.None},
                    (stream, encoding) => new HttpResponseStreamWriter(stream, Encoding.UTF8),
                    entity.GetType(),
                    entity));

                memoryStream.Seek(0, SeekOrigin.Begin);

                using (var streamReader = new StreamReader(memoryStream))
                {
                    Assert.Equal(JsonConvert.SerializeObject(entity,
                        new JsonSerializerSettings
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver(),
                            NullValueHandling = NullValueHandling.Ignore
                        }), streamReader.ReadToEnd());
                }
            }
        }

        [Fact]
        public void it_cant_write_object_that_is_not_entity()
        {
            var canWriteResult = _sirenOutputFormatter.CanWriteResult(new OutputFormatterWriteContext(
                new DefaultHttpContext(),
                (stream, encoding) => TextWriter.Null,
                typeof(object),
                null));

            Assert.False(canWriteResult);
        }

        [Fact]
        public async Task it_cant_write_to_stream_when_request_aborted()
        {
            using (var cancellationTokenSource = new CancellationTokenSource(0))
            {
                var entity = new EntityBuilder().WithClass("class").Build();

                await Assert.ThrowsAsync<TaskCanceledException>(() => _sirenOutputFormatter.WriteAsync(
                    new OutputFormatterWriteContext(
                        new DefaultHttpContext
                        {
                            RequestAborted = cancellationTokenSource.Token
                        },
                        (stream, encoding) => new HttpResponseStreamWriter(stream, Encoding.UTF8),
                        entity.GetType(),
                        entity)));
            }
        }
    }
}
using System.IO;
using System.Threading;
using FluentSiren.AspNet.WebApi.Formatting;
using FluentSiren.Builders;
using FluentSiren.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;

namespace FluentSiren.AspNet.WebApi.Tests.Unit.Formatting
{
    [TestFixture]
    public class SirenMediaFormatterTests
    {
        [SetUp]
        public void SetUp()
        {
            _sirenMediaFormatter = new SirenMediaFormatter();
        }

        private SirenMediaFormatter _sirenMediaFormatter;


        [Test]
        public void it_can_write_entity()
        {
            Assert.That(_sirenMediaFormatter.CanWriteType(typeof(Entity)), Is.True);
        }

        [Test]
        public void it_cant_read_entity()
        {
            Assert.That(_sirenMediaFormatter.CanReadType(typeof(Entity)), Is.False);
        }

        [Test]
        public void it_cant_read_object()
        {
            Assert.That(_sirenMediaFormatter.CanReadType(typeof(object)), Is.False);
        }

        [Test]
        public void it_cant_write_object_that_is_not_entity()
        {
            Assert.That(_sirenMediaFormatter.CanWriteType(typeof(object)), Is.False);
        }

        [Test]
        public void it_can_write_to_stream()
        {
            using (var memoryStream = new MemoryStream())
            {
                var entity = new EntityBuilder().Build();

                _sirenMediaFormatter.WriteToStreamAsync(entity.GetType(), entity, memoryStream, null, null).Wait();

                memoryStream.Seek(0, SeekOrigin.Begin);

                using (var streamReader = new StreamReader(memoryStream))
                {
                    Assert.That(streamReader.ReadToEnd(), Is.EqualTo(JsonConvert.SerializeObject(entity, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver(), NullValueHandling = NullValueHandling.Ignore})));
                }
            }
        }

        [Test]
        public void it_can_write_to_stream_with_cancellation_token()
        {
            using (var memoryStream = new MemoryStream())
            {
                var entity = new EntityBuilder().Build();

                _sirenMediaFormatter.WriteToStreamAsync(entity.GetType(), entity, memoryStream, null, null, CancellationToken.None).Wait();

                memoryStream.Seek(0, SeekOrigin.Begin);

                using (var streamReader = new StreamReader(memoryStream))
                {
                    Assert.That(streamReader.ReadToEnd(), Is.EqualTo(JsonConvert.SerializeObject(entity, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver(), NullValueHandling = NullValueHandling.Ignore})));
                }
            }
        }
    }
}
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentSiren.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FluentSiren.AspNet.WebApi.Formatting
{
    public class SirenMediaFormatter : MediaTypeFormatter
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public SirenMediaFormatter() : this(new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver(), NullValueHandling = NullValueHandling.Ignore})
        {
        }

        public SirenMediaFormatter(JsonSerializerSettings settings)
        {
            _jsonSerializerSettings = settings;
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            var serializeObject = SerializeObject(value);

            var bytes = GetBytes(serializeObject);

            return writeStream.WriteAsync(bytes, 0, bytes.Length);
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext, CancellationToken cancellationToken)
        {
            var serializeObject = SerializeObject(value);

            var bytes = GetBytes(serializeObject);

            return writeStream.WriteAsync(bytes, 0, bytes.Length, cancellationToken);
        }

        private string SerializeObject(object value)
        {
            return JsonConvert.SerializeObject(value, _jsonSerializerSettings);
        }

        private static byte[] GetBytes(string serializeObject)
        {
            return Encoding.UTF8.GetBytes(serializeObject);
        }

        public override bool CanReadType(Type type)
        {
            return false;
        }

        public override bool CanWriteType(Type type)
        {
            return type == typeof(Entity);
        }
    }
}
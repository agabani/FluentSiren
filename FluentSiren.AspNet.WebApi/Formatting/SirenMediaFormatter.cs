using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
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
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/vnd.siren+json"));
            _jsonSerializerSettings = settings;
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            return WriteToStreamAsync(type, value, writeStream, content, transportContext, CancellationToken.None);
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext, CancellationToken cancellationToken)
        {
            var serializeObject = SerializeObject(value);

            var bytes = Encoding.UTF8.GetBytes(serializeObject);

            return writeStream.WriteAsync(bytes, 0, bytes.Length, cancellationToken);
        }

        private string SerializeObject(object value)
        {
            return JsonConvert.SerializeObject(value, _jsonSerializerSettings);
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
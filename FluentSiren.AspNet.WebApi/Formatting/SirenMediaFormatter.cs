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
        private static readonly JsonSerializerSettings DefaultJsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };

        private static readonly Encoding DefaultEncoding = Encoding.UTF8;

        private readonly Encoding _encoding;
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public SirenMediaFormatter() : this(DefaultJsonSerializerSettings, DefaultEncoding)
        {
        }

        public SirenMediaFormatter(JsonSerializerSettings settings) : this(settings, DefaultEncoding)
        {
        }

        public SirenMediaFormatter(Encoding encoding) : this(DefaultJsonSerializerSettings, encoding)
        {
        }

        public SirenMediaFormatter(JsonSerializerSettings settings, Encoding encoding)
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/vnd.siren+json"));
            _jsonSerializerSettings = settings;
            _encoding = encoding;
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            return WriteToStreamAsync(type, value, writeStream, content, transportContext, CancellationToken.None);
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext, CancellationToken cancellationToken)
        {
            var serializeObject = SerializeObject(value);

            var bytes = _encoding.GetBytes(serializeObject);

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
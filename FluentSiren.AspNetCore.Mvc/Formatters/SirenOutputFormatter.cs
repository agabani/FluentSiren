using System;
using System.Text;
using System.Threading.Tasks;
using FluentSiren.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FluentSiren.AspNetCore.Mvc.Formatters
{
    public class SirenOutputFormatter : TextOutputFormatter
    {
        private static readonly JsonSerializerSettings DefaultJsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };

        private static readonly Encoding DefaultEncoding = Encoding.UTF8;

        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public SirenOutputFormatter() : this(DefaultJsonSerializerSettings, DefaultEncoding)
        {
        }

        public SirenOutputFormatter(JsonSerializerSettings settings) : this(settings, DefaultEncoding)
        {
        }

        public SirenOutputFormatter(Encoding encoding) : this(DefaultJsonSerializerSettings, encoding)
        {
        }

        public SirenOutputFormatter(JsonSerializerSettings settings, Encoding encoding)
        {
            _jsonSerializerSettings = settings;
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/vnd.siren+json"));
            SupportedEncodings.Add(encoding);
        }

        protected override bool CanWriteType(Type type)
        {
            return type == typeof(Entity);
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            return context.HttpContext.Response.WriteAsync(
                JsonConvert.SerializeObject(context.Object, _jsonSerializerSettings),
                selectedEncoding,
                context.HttpContext.RequestAborted);
        }
    }
}
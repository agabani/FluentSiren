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
        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };

        public SirenOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/vnd.siren+json"));
            SupportedEncodings.Add(Encoding.UTF8);
        }

        protected override bool CanWriteType(Type type)
        {
            return type == typeof(Entity);
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            return context.HttpContext.Response.WriteAsync(
                JsonConvert.SerializeObject(context.Object, JsonSerializerSettings),
                selectedEncoding,
                context.HttpContext.RequestAborted);
        }
    }
}
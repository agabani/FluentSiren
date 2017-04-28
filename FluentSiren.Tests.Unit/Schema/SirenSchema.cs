using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace FluentSiren.Tests.Unit.Schema
{
    public static class SirenSchema
    {
        private static readonly JSchema JSchema;

        static SirenSchema()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("FluentSiren.Tests.Unit.Schema.siren.schema.json"))
                // ReSharper disable once AssignNullToNotNullAttribute
            using (var reader = new StreamReader(stream))
            {
                JSchema = JSchema.Parse(reader.ReadToEnd());
            }
        }

        public static void Validate(string json)
        {
            IList<string> errorMessages;
            if (!JObject.Parse(json).IsValid(JSchema, out errorMessages))
                throw new Exception(string.Join(" | ", errorMessages));
        }
    }
}
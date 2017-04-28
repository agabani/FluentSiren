using System;
using FluentSiren.Builders;
using FluentSiren.Tests.Unit.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;

namespace FluentSiren.Tests.Unit.Examples
{
    [TestFixture]
    public class Example
    {
        [Test]
        public void Test()
        {
            var entity = new EntityBuilder()
                .WithClass("order")
                .WithProperty("orderNumber", 42)
                .WithProperty("itemCount", 3)
                .WithProperty("status", "pending")
                .WithSubEntity(new EmbeddedLinkBuilder()
                    .WithClass("items")
                    .WithClass("collection")
                    .WithRel("http://x.io/rels/order-items")
                    .WithHref("http://api.x.io/orders/42/items"))
                .WithSubEntity(new EmbeddedRepresentationBuilder()
                    .WithClass("info")
                    .WithClass("customer")
                    .WithRel("http://x.io/rels/customer")
                    .WithProperty("customerId", "pj123")
                    .WithProperty("name", "Peter Joseph")
                    .WithLink(new LinkBuilder()
                        .WithRel("self")
                        .WithHref("http://api.x.io/customers/pj123")))
                .WithAction(new ActionBuilder()
                    .WithName("add-item")
                    .WithTitle("Add Item")
                    .WithMethod("POST")
                    .WithHref("http://api.x.io/orders/42/items")
                    .WithType("application/x-www-form-urlencoded")
                    .WithField(new FieldBuilder()
                        .WithName("orderNumber")
                        .WithType("hidden")
                        .WithValue("42"))
                    .WithField(new FieldBuilder()
                        .WithName("productCode")
                        .WithType("text"))
                    .WithField(new FieldBuilder()
                        .WithName("quantity")
                        .WithType("number")))
                .WithLink(new LinkBuilder()
                    .WithRel("self")
                    .WithHref("http://api.x.io/orders/42"))
                .WithLink(new LinkBuilder()
                    .WithRel("previous")
                    .WithHref("http://api.x.io/orders/41"))
                .WithLink(new LinkBuilder()
                    .WithRel("next")
                    .WithHref("http://api.x.io/orders/43"))
                .Build();

            var json = JsonConvert.SerializeObject(entity, Formatting.Indented, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            });

            SirenSchema.Validate(json);

            Console.WriteLine(json);
        }
    }
}
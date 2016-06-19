# FluentSiren: A fluent API to build specification enforced siren entities.

__Current specification version: 0.6.1__

- [Offical Siren Specification](https://github.com/kevinswiber/siren)

[![Build status](https://ci.appveyor.com/api/projects/status/wjsu38ekwr5593r3/branch/master?svg=true)](https://ci.appveyor.com/project/AhmedAgabani/fluentsiren/branch/master)

|Package                  |Version                                                                                                                             |
|-------------------------|------------------------------------------------------------------------------------------------------------------------------------|
|FluentSiren              |[![NuGet downloads](https://img.shields.io/badge/nuget-v0.6.1.0-blue.svg)](https://www.nuget.org/packages/FluentSiren)              |
|FluentSiren.AspNet.WebApi|[![NuGet downloads](https://img.shields.io/badge/nuget-v0.6.1.0-blue.svg)](https://www.nuget.org/packages/FluentSiren.AspNet.WebApi)|

## Example
Below is a C# example that builds the JSON Siren example found on the [Offical Siren Specification](https://github.com/kevinswiber/siren).
```csharp
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

Console.WriteLine(json);			
```

```json
{
  "class": [
    "order"
  ],
  "properties": {
    "orderNumber": 42,
    "itemCount": 3,
    "status": "pending"
  },
  "entities": [
    {
      "class": [
        "items",
        "collection"
      ],
      "rel": [
        "http://x.io/rels/order-items"
      ],
      "href": "http://api.x.io/orders/42/items"
    },
    {
      "class": [
        "info",
        "customer"
      ],
      "rel": [
        "http://x.io/rels/customer"
      ],
      "properties": {
        "customerId": "pj123",
        "name": "Peter Joseph"
      },
      "links": [
        {
          "rel": [
            "self"
          ],
          "href": "http://api.x.io/customers/pj123"
        }
      ]
    }
  ],
  "links": [
    {
      "rel": [
        "self"
      ],
      "href": "http://api.x.io/orders/42"
    },
    {
      "rel": [
        "previous"
      ],
      "href": "http://api.x.io/orders/41"
    },
    {
      "rel": [
        "next"
      ],
      "href": "http://api.x.io/orders/43"
    }
  ],
  "actions": [
    {
      "name": "add-item",
      "method": "POST",
      "href": "http://api.x.io/orders/42/items",
      "title": "Add Item",
      "type": "application/x-www-form-urlencoded",
      "fields": [
        {
          "name": "orderNumber",
          "type": "hidden",
          "value": "42"
        },
        {
          "name": "productCode",
          "type": "text"
        },
        {
          "name": "quantity",
          "type": "number"
        }
      ]
    }
  ]
}
```

## WebApi

To use FluentSiren in a WebApi project install FluentSiren.AspNet.WebApi package through the "NuGet Package Manager" or the "Package Manager Console" by typing the following command `install-package FluentSiren.AspNet.WebApi`

Once the package is installed, add `config.Formatters.Add(new SirenMediaFormatter());` to your `WebApiConfig.cs` as shown in the example below.

```csharp
using System.Web.Http;
using FluentSiren.AspNet.WebApi.Formatting;

namespace Demo.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Add this line below to WebApiConfig.cs
            config.Formatters.Add(new SirenMediaFormatter());

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional }
            );
        }
    }
}
```

Once all these steps have been done, any incoming HTTP request with the ACCEPT header `application/vnd.siren+json` will be served a Siren entity.
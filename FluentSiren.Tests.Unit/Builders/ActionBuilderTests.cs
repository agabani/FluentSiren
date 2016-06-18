using System;
using System.Linq;
using FluentSiren.Builders;
using NUnit.Framework;

namespace FluentSiren.Tests.Unit.Builders
{
    [TestFixture]
    public class ActionBuilderTests
    {
        [SetUp]
        public void SetUp()
        {
            _builder = new ActionBuilder();
        }

        private ActionBuilder _builder;

        [Test]
        public void it_can_build()
        {
            var action = _builder
                .WithName("name")
                .WithClass("class 1")
                .WithClass("class 2")
                .WithMethod("method")
                .WithHref("href")
                .WithTitle("title")
                .WithType("type")
                .WithField(new FieldBuilder().WithName("name 1"))
                .WithField(new FieldBuilder().WithName("name 2"))
                .Build();

            Assert.That(action.Name, Is.EqualTo("name"));
            Assert.That(action.Class.Select(x => x), Is.EqualTo(new[] { "class 1", "class 2" }));
            Assert.That(action.Method, Is.EqualTo("method"));
            Assert.That(action.Href, Is.EqualTo("href"));
            Assert.That(action.Title, Is.EqualTo("title"));
            Assert.That(action.Type, Is.EqualTo("type"));
            Assert.That(action.Fields.Select(x => x.Name), Is.EqualTo(new[] { "name 1", "name 2" }));
        }

        [Test]
        public void it_does_not_share_references()
        {
            _builder
                .WithName("name")
                .WithHref("href")
                .WithClass("class")
                .WithField(new FieldBuilder().WithName("name"));

            var action1 = _builder.Build();
            var action2 = _builder.Build();

            Assert.That(action1, Is.Not.SameAs(action2));
            Assert.That(action1.Class, Is.Not.SameAs(action2.Class));
            Assert.That(action1.Fields, Is.Not.SameAs(action2.Fields));
        }

        [Test]
        public void name_is_required()
        {
            Assert.That(Assert.Throws<ArgumentException>(() => _builder.WithHref("href").Build()).Message, Is.EqualTo("Name is required."));
        }

        [Test]
        public void class_is_optional()
        {
            Assert.That(_builder.WithName("name").WithHref("href").Build().Class, Is.Null);
        }

        [Test]
        public void method_is_optional_and_defaults_to_get()
        {
            Assert.That(_builder.WithName("name").WithHref("href").Build().Method, Is.EqualTo("GET"));
        }

        [Test]
        public void href_is_required()
        {
            Assert.That(Assert.Throws<ArgumentException>(() => _builder.WithName("name").Build()).Message, Is.EqualTo("Href is required."));
        }

        [Test]
        public void title_is_optional()
        {
            Assert.That(_builder.WithName("name").WithHref("href").Build().Title, Is.Null);
        }

        [Test]
        public void type_is_optional()
        {
            Assert.That(_builder.WithName("name").WithHref("href").Build().Type, Is.Null);
        }

        [Test]
        public void type_is_optional_and_defaults_to_application_x_www_form_urlencoded_when_field_exists()
        {
            Assert.That(_builder.WithName("name").WithHref("href").WithField(new FieldBuilder().WithName("name")).Build().Type, Is.EqualTo("application/x-www-form-urlencoded"));
        }

        [Test]
        public void field_is_optional()
        {
            Assert.That(_builder.WithName("name").WithHref("href").Build().Fields, Is.Null);
        }

        [Test]
        public void fields_must_have_unique_names()
        {
            Assert.That(Assert.Throws<ArgumentException>(() => _builder.WithName("name").WithHref("href").WithField(new FieldBuilder().WithName("name")).WithField(new FieldBuilder().WithName("name")).Build()).Message, Is.EqualTo("Field names MUST be unique within the set of fields for an action."));
        }
    }
}

using System;
using System.Linq;
using FluentSiren.Builders;
using NUnit.Framework;

namespace FluentSiren.Tests.Unit.Builders
{
    [TestFixture]
    public class FieldBuilderTests
    {
        [SetUp]
        public void SetUp()
        {
            _builder = new FieldBuilder();
        }

        private FieldBuilder _builder;

        [Test]
        public void it_can_build()
        {
            var field = _builder
                .WithName("name")
                .WithClass("class 1")
                .WithClass("class 2")
                .WithType("type")
                .WithValue("value")
                .WithTitle("title")
                .Build();

            Assert.That(field.Name, Is.EqualTo("name"));
            Assert.That(field.Class.Select(x => x), Is.EqualTo(new [] {"class 1", "class 2"}));
            Assert.That(field.Type, Is.EqualTo("type"));
            Assert.That(field.Value, Is.EqualTo("value"));
            Assert.That(field.Title, Is.EqualTo("title"));
        }

        [Test]
        public void it_does_not_share_references()
        {
            _builder
                .WithName("name")
                .WithClass("class");

            var field1 = _builder.Build();
            var field2 = _builder.Build();

            Assert.That(field1, Is.Not.SameAs(field2));
            Assert.That(field1.Class, Is.Not.SameAs(field2.Class));
        }

        [Test]
        public void name_is_required()
        {
            Assert.That(Assert.Throws<ArgumentException>(() => _builder.Build()).Message, Is.EqualTo("Name is required."));
        }

        [Test]
        public void class_is_optional()
        {
            Assert.That(_builder.WithName("name").Build().Class, Is.Null);
        }

        [Test]
        public void type_is_optional_and_defaults_to_text()
        {
            Assert.That(_builder.WithName("name").Build().Type, Is.EqualTo("text"));
        }

        [Test]
        public void value_is_optional()
        {
            Assert.That(_builder.WithName("name").Build().Value, Is.Null);
        }

        [Test]
        public void title_is_optional()
        {
            Assert.That(_builder.WithName("name").Build().Title, Is.Null);
        }
    }
}
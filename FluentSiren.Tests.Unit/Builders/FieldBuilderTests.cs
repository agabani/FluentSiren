using System;
using System.Linq;
using FluentSiren.Builders;
using FluentSiren.Models;
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

        private enum Enum
        {
            One,
            Two,
            Three
        }

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
            Assert.That(field.Class.Select(x => x), Is.EqualTo(new[] {"class 1", "class 2"}));
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
        public void value_can_be_a_number_decimal()
        {
            Assert.That(_builder.WithName("name").WithValue((decimal) 1).Build().Value, Is.EqualTo(1));
        }

        [Test]
        [TestCase("string")]
        [TestCase((byte) 1)]
        [TestCase('a')]
        [TestCase(Enum.Two)]
        [TestCase((float) 1)]
        [TestCase((int) 1.0)]
        [TestCase((long) 1)]
        [TestCase((sbyte) 1)]
        [TestCase((short) 1)]
        [TestCase((uint) 1)]
        [TestCase((ulong) 1)]
        [TestCase((ushort) 1)]
        public void value_can_be_a_number_or_a_string(object value)
        {
            Assert.That(_builder.WithName("name").WithValue(value).Build().Value, Is.EqualTo(value));
        }

        [Test]
        public void value_can_be_a_field_value()
        {
            Assert.That(((FieldValue[]) _builder.WithName("name").WithValue(new FieldValueBuilder().WithValue("value")).Build().Value).Single().Value, Is.EqualTo("value"));
        }

        [Test]
        public void title_is_optional()
        {
            Assert.That(_builder.WithName("name").Build().Title, Is.Null);
        }

        [Test]
        public void value_can_not_be_a_bool()
        {
            Assert.That(Assert.Throws<ArgumentException>(() => _builder.WithName("name").WithValue(true).Build()).Message, Is.EqualTo("Value must be a string, a number, or a list of field values."));
        }

        [Test]
        public void value_can_not_be_an_object()
        {
            Assert.That(Assert.Throws<ArgumentException>(() => _builder.WithName("name").WithValue(new object()).Build()).Message, Is.EqualTo("Value must be a string, a number, or a list of field values."));
        }
    }
}
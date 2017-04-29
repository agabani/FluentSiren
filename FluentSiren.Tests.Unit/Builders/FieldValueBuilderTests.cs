using System;
using FluentSiren.Builders;
using NUnit.Framework;

namespace FluentSiren.Tests.Unit.Builders
{
    [TestFixture]
    public class FieldValueBuilderTests
    {
        [SetUp]
        public void SetUp()
        {
            _builder = new FieldValueBuilder();
        }

        private FieldValueBuilder _builder;

        private enum Enum
        {
            One,
            Two,
            Three
        }

        [Test]
        public void it_can_build()
        {
            var fieldValue = _builder
                .WithTitle("title")
                .WithValue("value")
                .WithSelected(true)
                .Build();

            Assert.That(fieldValue.Title, Is.EqualTo("title"));
            Assert.That(fieldValue.Value, Is.EqualTo("value"));
            Assert.That(fieldValue.Selected, Is.True);
        }

        [Test]
        public void it_does_not_share_references()
        {
            var fieldValue1 = _builder.WithValue("value").Build();
            var fieldValue2 = _builder.WithValue("value").Build();

            Assert.That(fieldValue1, Is.Not.SameAs(fieldValue2));
        }

        [Test]
        public void title_is_optional()
        {
            Assert.That(_builder.WithValue("value").Build().Title, Is.Null);
        }

        [Test]
        public void value_is_required()
        {
            Assert.That(Assert.Throws<ArgumentException>(() => _builder.Build()).Message, Is.EqualTo("Value is required."));
        }

        [Test]
        public void selected_is_optional_and_defaults_to_false()
        {
            Assert.That(_builder.WithValue("value").Build().Selected, Is.False);
        }

        [Test]
        public void value_can_be_a_number_decimal()
        {
            Assert.That(_builder.WithValue((decimal)1).Build().Value, Is.EqualTo(1));
        }

        [Test]
        [TestCase("string")]
        [TestCase((byte)1)]
        [TestCase('a')]
        [TestCase(Enum.Two)]
        [TestCase((float)1)]
        [TestCase((int)1.0)]
        [TestCase((long)1)]
        [TestCase((sbyte)1)]
        [TestCase((short)1)]
        [TestCase((uint)1)]
        [TestCase((ulong)1)]
        [TestCase((ushort)1)]
        public void value_can_be_a_number_or_a_string(object value)
        {
            Assert.That(_builder.WithValue(value).Build().Value, Is.EqualTo(value));
        }

        [Test]
        public void value_can_not_be_a_bool()
        {
            Assert.That(Assert.Throws<ArgumentException>(() => _builder.WithValue(true).Build()).Message, Is.EqualTo("Value must be a string or a number."));
        }

        [Test]
        public void value_can_not_be_an_object()
        {
            Assert.That(Assert.Throws<ArgumentException>(() => _builder.WithValue(new object()).Build()).Message, Is.EqualTo("Value must be a string or a number."));
        }
    }
}
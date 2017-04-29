using System;
using System.Linq;
using FluentSiren.Builders;
using FluentSiren.Enums;
using NUnit.Framework;

namespace FluentSiren.Tests.Unit.Builders
{
    public class LinkBuilderTests
    {
        [SetUp]
        public void SetUp()
        {
            _builder = new LinkBuilder();
        }

        private LinkBuilder _builder;

        [Test]
        public void it_can_build()
        {
            var link = _builder
                .WithRel(Rel.Item)
                .WithRel(Rel.First)
                .WithClass("class 1")
                .WithClass("class 2")
                .WithHref(new Uri("http://href"))
                .WithTitle("title")
                .WithType("type")
                .Build();

            Assert.That(link.Rel.Select(x => x), Is.EqualTo(new[] { "item", "first" }));
            Assert.That(link.Class.Select(x => x), Is.EqualTo(new[] { "class 1", "class 2" }));
            Assert.That(link.Href, Is.EqualTo("http://href/"));
            Assert.That(link.Title, Is.EqualTo("title"));
            Assert.That(link.Type, Is.EqualTo("type"));
        }

        [Test]
        public void it_does_not_share_references()
        {
            _builder
                .WithRel(Rel.Item)
                .WithHref(new Uri("http://href"))
                .WithClass("class");

            var link1 = _builder.Build();
            var link2 = _builder.Build();

            Assert.That(link1, Is.Not.SameAs(link2));
            Assert.That(link1.Rel, Is.Not.SameAs(link2.Rel));
            Assert.That(link1.Class, Is.Not.SameAs(link2.Class));
        }

        [Test]
        public void rel_is_required()
        {
            Assert.That(Assert.Throws<ArgumentException>(() => _builder.WithHref(new Uri("http://href")).Build()).Message, Is.EqualTo("Rel is required."));
        }

        [Test]
        public void class_is_optional()
        {
            Assert.That(_builder.WithRel(Rel.Item).WithHref(new Uri("http://href")).Build().Class, Is.Null);
        }

        [Test]
        public void href_is_required()
        {
            Assert.That(Assert.Throws<ArgumentException>(() => _builder.WithRel(Rel.Item).Build()).Message, Is.EqualTo("Href is required."));
        }

        [Test]
        public void title_is_optional()
        {
            Assert.That(_builder.WithRel(Rel.Item).WithHref(new Uri("http://href")).Build().Title, Is.Null);
        }

        [Test]
        public void type_is_optional()
        {
            Assert.That(_builder.WithRel(Rel.Item).WithHref(new Uri("http://href")).Build().Type, Is.Null);
        }
    }
}
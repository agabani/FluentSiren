﻿using System;
using System.Linq;
using FluentSiren.Builders;
using FluentSiren.Enums;
using NUnit.Framework;

namespace FluentSiren.Tests.Unit.Builders
{
    [TestFixture]
    public class EmbeddedLinkBuilderTests
    {
        [SetUp]
        public void SetUp()
        {
            _builder = new EmbeddedLinkBuilder();
        }

        private EmbeddedLinkBuilder _builder;

        [Test]
        public void it_can_build()
        {
            var subEntity = _builder
                .WithClass("class 1")
                .WithClass("class 2")
                .WithRel(Rel.Item)
                .WithRel(Rel.First)
                .WithHref(new Uri("http://href"))
                .WithType("type")
                .WithTitle("title")
                .Build();

            Assert.That(subEntity.Class.Select(x => x), Is.EqualTo(new[] {"class 1", "class 2"}));
            Assert.That(subEntity.Rel.Select(x => x), Is.EqualTo(new[] {"item", "first"}));
            Assert.That(subEntity.Href, Is.EqualTo("http://href/"));
            Assert.That(subEntity.Type, Is.EqualTo("type"));
            Assert.That(subEntity.Title, Is.EqualTo("title"));
        }

        [Test]
        public void it_does_not_share_references()
        {
            _builder
                .WithRel(Rel.Item)
                .WithHref(new Uri("http://href"))
                .WithClass("class");

            var subEntity1 = _builder.Build();
            var subEntity2 = _builder.Build();

            Assert.That(subEntity1, Is.Not.SameAs(subEntity2));
            Assert.That(subEntity1.Class, Is.Not.SameAs(subEntity2.Class));
            Assert.That(subEntity1.Rel, Is.Not.SameAs(subEntity2.Rel));
        }

        [Test]
        public void class_is_optional()
        {
            Assert.That(_builder.WithRel(Rel.Item).WithHref(new Uri("http://href")).Build().Class, Is.Null);
        }

        [Test]
        public void rel_is_required()
        {
            Assert.That(Assert.Throws<ArgumentException>(() => _builder.WithHref(new Uri("http://href")).Build()).Message, Is.EqualTo("Rel is required."));
        }

        [Test]
        public void href_is_required()
        {
            Assert.That(Assert.Throws<ArgumentException>(() => _builder.WithRel(Rel.Item).Build()).Message, Is.EqualTo("Href is required."));
        }

        [Test]
        public void type_is_optional()
        {
            Assert.That(_builder.WithRel(Rel.Item).WithHref(new Uri("http://href")).Build().Type, Is.Null);
        }

        [Test]
        public void title_is_optional()
        {
            Assert.That(_builder.WithRel(Rel.Item).WithHref(new Uri("http://href")).Build().Title, Is.Null);
        }
    }
}
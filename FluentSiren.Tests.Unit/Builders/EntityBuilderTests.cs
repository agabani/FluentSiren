﻿using System;
using System.Linq;
using FluentSiren.Builders;
using FluentSiren.Enums;
using NUnit.Framework;

namespace FluentSiren.Tests.Unit.Builders
{
    [TestFixture]
    public class EntityBuilderTests
    {
        [SetUp]
        public void SetUp()
        {
            _builder = new EntityBuilder();
        }

        private EntityBuilder _builder;

        [Test]
        public void it_can_build()
        {
            var entity = _builder
                .WithClass("class 1")
                .WithClass("class 2")
                .WithTitle("title")
                .WithProperty("key 1", "value 1")
                .WithProperty("key 2", "value 2")
                .WithSubEntity(new EmbeddedLinkBuilder().WithRel(Rel.Item).WithHref(new Uri("http://href")))
                .WithSubEntity(new EmbeddedLinkBuilder().WithRel(Rel.First).WithHref(new Uri("http://href")))
                .WithSubEntity(new EmbeddedRepresentationBuilder().WithRel(Rel.Index))
                .WithSubEntity(new EmbeddedRepresentationBuilder().WithRel(Rel.About))
                .WithLink(new LinkBuilder().WithRel(Rel.Item).WithHref(new Uri("http://href")))
                .WithLink(new LinkBuilder().WithRel(Rel.First).WithHref(new Uri("http://href")))
                .WithAction(new ActionBuilder().WithName("name 1").WithHref(new Uri("http://href")))
                .WithAction(new ActionBuilder().WithName("name 2").WithHref(new Uri("http://href")))
                .Build();

            Assert.That(entity.Class.Select(x => x), Is.EqualTo(new[] {"class 1", "class 2"}));
            Assert.That(entity.Properties.Select(x => $"{x.Key}:{x.Value}"), Is.EqualTo(new[] {"key 1:value 1", "key 2:value 2"}));
            Assert.That(entity.Entities.Select(x => x.Rel.Single()), Is.EqualTo(new[] { "item", "first", "index", "about" }));
            Assert.That(entity.Links.Select(x => x.Rel.Single()), Is.EqualTo(new[] { "item", "first" }));
            Assert.That(entity.Actions.Select(x => x.Name), Is.EqualTo(new[] {"name 1", "name 2"}));
            Assert.That(entity.Title, Is.EqualTo("title"));
        }

        [Test]
        public void it_does_not_share_references()
        {
            _builder
                .WithClass("class")
                .WithProperty("key", "value")
                .WithSubEntity(new EmbeddedRepresentationBuilder().WithRel(Rel.Item))
                .WithLink(new LinkBuilder().WithRel(Rel.Item).WithHref(new Uri("http://href")))
                .WithAction(new ActionBuilder().WithName("name").WithHref(new Uri("http://href")));

            var entity1 = _builder.Build();
            var entity2 = _builder.Build();

            Assert.That(entity1, Is.Not.SameAs(entity2));
            Assert.That(entity1.Class, Is.Not.SameAs(entity2.Class));
            Assert.That(entity1.Properties, Is.Not.SameAs(entity2.Properties));
            Assert.That(entity1.Links, Is.Not.SameAs(entity2.Links));
            Assert.That(entity1.Actions, Is.Not.SameAs(entity2.Actions));
        }

        [Test]
        public void class_is_optional()
        {
            Assert.That(_builder.Build().Class, Is.Null);
        }

        [Test]
        public void properties_is_optional()
        {
            Assert.That(_builder.Build().Properties, Is.Null);
        }

        [Test]
        public void entities_is_optional()
        {
            Assert.That(_builder.Build().Entities, Is.Null);
        }

        [Test]
        public void links_is_optional()
        {
            Assert.That(_builder.Build().Links, Is.Null);
        }

        [Test]
        public void actions_is_optional()
        {
            Assert.That(_builder.Build().Actions, Is.Null);
        }

        [Test]
        public void title_is_optional()
        {
            Assert.That(_builder.Build().Title, Is.Null);
        }

        [Test]
        public void actions_must_have_unique_names()
        {
            Assert.That(Assert.Throws<ArgumentException>(() => _builder.WithAction(new ActionBuilder().WithName("name").WithHref(new Uri("http://href"))).WithAction(new ActionBuilder().WithName("name").WithHref(new Uri("http://href"))).Build()).Message, Is.EqualTo("Action names MUST be unique within the set of actions for an entity."));
        }
    }
}
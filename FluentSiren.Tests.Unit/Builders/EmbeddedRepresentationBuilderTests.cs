using System;
using System.Linq;
using FluentSiren.Builders;
using FluentSiren.Enums;
using NUnit.Framework;

namespace FluentSiren.Tests.Unit.Builders
{
    [TestFixture]
    public class EmbeddedRepresentationBuilderTests
    {
        [SetUp]
        public void SetUp()
        {
            _builder = new EmbeddedRepresentationBuilder();
        }

        private EmbeddedRepresentationBuilder _builder;

        [Test]
        public void it_can_build()
        {
            var subEntity = _builder
                .WithClass("class 1")
                .WithClass("class 2")
                .WithRel(Rel.Item)
                .WithRel(Rel.First)
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
                .WithTitle("title")
                .Build();

            Assert.That(subEntity.Class.Select(x => x), Is.EqualTo(new[] {"class 1", "class 2"}));
            Assert.That(subEntity.Rel.Select(x => x), Is.EqualTo(new[] {"item", "first"}));
            Assert.That(subEntity.Properties.Select(x => $"{x.Key}:{x.Value}"), Is.EqualTo(new[] {"key 1:value 1", "key 2:value 2"}));
            Assert.That(subEntity.Entities.Select(x => x.Rel.Single()), Is.EqualTo(new[] {"item", "first", "index", "about"}));
            Assert.That(subEntity.Links.Select(x => x.Rel.Single()), Is.EqualTo(new[] {"item", "first"}));
            Assert.That(subEntity.Actions.Select(x => x.Name), Is.EqualTo(new[] {"name 1", "name 2"}));
            Assert.That(subEntity.Title, Is.EqualTo("title"));
        }

        [Test]
        public void it_does_not_share_references()
        {
            _builder
                .WithClass("class")
                .WithRel(Rel.Item)
                .WithProperty("key", "value")
                .WithSubEntity(new EmbeddedRepresentationBuilder().WithRel(Rel.Item))
                .WithLink(new LinkBuilder().WithRel(Rel.Item).WithHref(new Uri("http://href")))
                .WithAction(new ActionBuilder().WithName("name").WithHref(new Uri("http://href")));

            var subEntity1 = _builder.Build();
            var subEntity2 = _builder.Build();

            Assert.That(subEntity1, Is.Not.SameAs(subEntity2));
            Assert.That(subEntity1.Class, Is.Not.SameAs(subEntity2.Class));
            Assert.That(subEntity1.Rel, Is.Not.SameAs(subEntity2.Rel));
            Assert.That(subEntity1.Properties, Is.Not.SameAs(subEntity2.Properties));
            Assert.That(subEntity1.Links, Is.Not.SameAs(subEntity2.Links));
            Assert.That(subEntity1.Actions, Is.Not.SameAs(subEntity2.Actions));
        }

        [Test]
        public void class_is_optional()
        {
            Assert.That(_builder.WithRel(Rel.Item).Build().Class, Is.Null);
        }

        [Test]
        public void rel_is_required()
        {
            Assert.That(Assert.Throws<ArgumentException>(() => _builder.Build()).Message, Is.EqualTo("Rel is required."));
        }

        [Test]
        public void properties_is_optional()
        {
            Assert.That(_builder.WithRel(Rel.Item).Build().Properties, Is.Null);
        }

        [Test]
        public void entities_is_optional()
        {
            Assert.That(_builder.WithRel(Rel.Item).Build().Entities, Is.Null);
        }

        [Test]
        public void links_is_optional()
        {
            Assert.That(_builder.WithRel(Rel.Item).Build().Links, Is.Null);
        }

        [Test]
        public void actions_is_optional()
        {
            Assert.That(_builder.WithRel(Rel.Item).Build().Actions, Is.Null);
        }

        [Test]
        public void title_is_optional()
        {
            Assert.That(_builder.WithRel(Rel.Item).Build().Title, Is.Null);
        }

        [Test]
        public void actions_must_have_unique_names()
        {
            Assert.That(Assert.Throws<ArgumentException>(() => _builder.WithRel(Rel.Item).WithAction(new ActionBuilder().WithName("name").WithHref(new Uri("http://href"))).WithAction(new ActionBuilder().WithName("name").WithHref(new Uri("http://href"))).Build()).Message, Is.EqualTo("Action names MUST be unique within the set of actions for an entity."));
        }
    }
}
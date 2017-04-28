using System;
using System.Collections.Generic;
using System.Linq;
using FluentSiren.Builders;
using FluentSiren.Models;
using NUnit.Framework;

namespace FluentSiren.Tests.Unit.Examples
{
    [TestFixture]
    public class TemplatelessExample
    {
        [Test]
        public void Test()
        {
            var items = new List<Person>
            {
                new Person(Guid.NewGuid(), "", DateTime.UtcNow),
                new Person(Guid.NewGuid(), "", DateTime.UtcNow),
                new Person(Guid.NewGuid(), "", DateTime.UtcNow)
            };

            var entity = items.First()
                .ToEntity()
                .WithClass("collection")
                .WithProperty("count", items.Count);

            foreach (var item in items)
                entity.WithSubEntity(item.ToRepresentation().WithRel("item"));

            entity.Build();
        }

        internal class Person
        {
            internal Person(Guid id, string name, DateTime dateOfBirth)
            {
                Id = id;
                Name = name;
                DateOfBirth = dateOfBirth;
            }

            internal Guid Id { get; }
            internal string Name { get; }
            internal DateTime DateOfBirth { get; }
        }
    }

    internal static class PersonExtention
    {
        internal static EntityBuilder ToEntity(this TemplatelessExample.Person person)
        {
            return (EntityBuilder) Build(new EntityBuilder(), person);
        }

        internal static EmbeddedRepresentationBuilder ToRepresentation(this TemplatelessExample.Person person)
        {
            return (EmbeddedRepresentationBuilder) Build(new EmbeddedRepresentationBuilder(), person);
        }

        private static EntityBuilder<TBuilder, Entity> Build<TBuilder>(EntityBuilder<TBuilder, Entity> builder, TemplatelessExample.Person person)
            where TBuilder : EntityBuilder<TBuilder, Entity>
        {
            return builder
                .WithClass("person")
                .WithProperty("name", person.Name)
                .WithProperty("date.of.birth", DateTime.Now);
        }
    }
}
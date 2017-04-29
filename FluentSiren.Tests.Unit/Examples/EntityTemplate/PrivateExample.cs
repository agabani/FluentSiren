using System;
using System.Collections.Generic;
using System.Linq;
using FluentSiren.Builders;
using FluentSiren.Enums;
using FluentSiren.Models;
using NUnit.Framework;

namespace FluentSiren.Tests.Unit.Examples.EntityTemplate
{
    [TestFixture]
    public class PrivateExample
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

            var entity = ToEntity(items.First())
                .WithClass("collection")
                .WithProperty("count", items.Count);

            foreach (var item in items)
                entity.WithSubEntity(ToRepresentation(item).WithRel(Rel.Item));

            entity.Build();
        }

        private class Person
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

        private static EntityBuilder ToEntity(Person person)
        {
            return (EntityBuilder) Build<EntityBuilder>(person);
        }

        private static EmbeddedRepresentationBuilder ToRepresentation(Person person)
        {
            return (EmbeddedRepresentationBuilder) Build<EmbeddedRepresentationBuilder>(person);
        }

        private static EntityBuilder<TBuilder, Entity> Build<TBuilder>(Person person)
            where TBuilder : EntityBuilder<TBuilder, Entity>
        {
            return Activator.CreateInstance<TBuilder>()
                .WithClass("person")
                .WithProperty("id", person.Id)
                .WithProperty("name", person.Name)
                .WithProperty("date.of.birth", person.DateOfBirth);
        }
    }
}
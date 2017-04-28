using System;
using System.Collections.Generic;
using System.Linq;
using FluentSiren.Builders;
using FluentSiren.Models;
using FluentSiren.Templates;
using NUnit.Framework;

namespace FluentSiren.Tests.Unit.Examples.EntityTemplate
{
    [TestFixture]
    public class TemplateExample
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

            var template = new PersonEntityTemplate();

            var entity = template
                .ToEntity(items.First())
                .WithClass("person")
                .WithClass("collection")
                .WithProperty("count", items.Count);

            foreach (var item in items)
                entity.WithSubEntity(template.ToRepresentation(item).WithRel("item"));

            entity.Build();
        }

        public class Person
        {
            public Person(Guid id, string name, DateTime dateOfBirth)
            {
                Id = id;
                Name = name;
                DateOfBirth = dateOfBirth;
            }

            public Guid Id { get; }
            public string Name { get; }
            public DateTime DateOfBirth { get; }
        }

        public class PersonEntityTemplate : EntityTemplate<Person>
        {
            protected override EntityBuilder<TBuilder, Entity> Build<TBuilder>(EntityBuilder<TBuilder, Entity> builder, Person person)
            {
                return builder
                    .WithClass("person")
                    .WithProperty("id", person.Id)
                    .WithProperty("name", person.Name)
                    .WithProperty("date.of.birth", person.DateOfBirth);
            }
        }
    }
}
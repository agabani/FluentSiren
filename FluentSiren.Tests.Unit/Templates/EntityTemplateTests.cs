using System;
using System.Linq;
using FluentSiren.Builders;
using FluentSiren.Models;
using FluentSiren.Templates;
using NUnit.Framework;

namespace FluentSiren.Tests.Unit.Templates
{
    [TestFixture]
    public class EntityTemplateTests
    {
        private Person _person;
        private PersonEntityTemplate _template;

        private class Person
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

        private class PersonEntityTemplate : EntityTemplate<Person>
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

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _person = new Person(Guid.NewGuid(), "Name", DateTime.UtcNow);
            _template = new PersonEntityTemplate();
        }


        [Test]
        public void it_can_create_an_embedded_representation()
        {
            var builder = _template.ToRepresentation(_person);

            Assert.IsInstanceOf<EmbeddedRepresentationBuilder>(builder);

            var entity = builder.WithRel("rel").Build();

            Assert.That(entity.Rel.Single(), Is.EqualTo("rel"));
            Assert.That(entity.Properties["id"], Is.EqualTo(_person.Id));
            Assert.That(entity.Properties["name"], Is.EqualTo(_person.Name));
            Assert.That(entity.Properties["date.of.birth"], Is.EqualTo(_person.DateOfBirth));
        }

        [Test]
        public void it_can_create_an_entity()
        {
            var builder = _template.ToEntity(_person);

            Assert.IsInstanceOf<EntityBuilder>(builder);

            var entity = builder.Build();

            Assert.That(entity.Properties["id"], Is.EqualTo(_person.Id));
            Assert.That(entity.Properties["name"], Is.EqualTo(_person.Name));
            Assert.That(entity.Properties["date.of.birth"], Is.EqualTo(_person.DateOfBirth));
        }
    }
}
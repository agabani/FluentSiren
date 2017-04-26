using FluentSiren.Builders;
using FluentSiren.Models;

namespace FluentSiren.Templates
{
    public abstract class EntityTemplate<T> where T : class
    {
        public EntityBuilder ToEntity(T model)
        {
            return (EntityBuilder) Build(new EntityBuilder(), model);
        }

        public EmbeddedRepresentationBuilder ToRepresentation(T model)
        {
            return (EmbeddedRepresentationBuilder) Build(new EmbeddedRepresentationBuilder(), model);
        }

        protected abstract EntityBuilder<TBuilder, Entity> Build<TBuilder>(EntityBuilder<TBuilder, Entity> builder, T model) where TBuilder : EntityBuilder<TBuilder, Entity>;
    }
}
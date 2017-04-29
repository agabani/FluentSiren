using System;
using System.Collections.Generic;
using System.Linq;
using FluentSiren.Enums;
using FluentSiren.Models;

namespace FluentSiren.Builders
{
    public class EmbeddedRepresentationBuilder : EmbeddedRepresentationBuilder<EmbeddedRepresentationBuilder, Entity>, ISubEntityBuilder
    {
    }

    public class EmbeddedRepresentationBuilder<TBuilder, TEntity> : EntityBuilder<TBuilder, TEntity>
        where TBuilder : EmbeddedRepresentationBuilder<TBuilder, TEntity>
        where TEntity : Entity
    {
        private readonly List<object> _rel = new List<object>();

        public TBuilder WithRel(Uri rel)
        {
            _rel.Add(rel);
            return This;
        }

        public TBuilder WithRel(Rel rel)
        {
            _rel.Add(rel);
            return This;
        }

        public override TEntity Build()
        {
            if (!_rel.Any())
                throw new ArgumentException("Rel is required.");

            var subEntity = new Entity
            {
                Class = Class?.ToArray(),
                Rel = _rel.Select(x => x is Rel ? ((Rel)x).GetName() : ((Uri)x).ToString()).ToArray(),
                Properties = Properties?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
                Entities = SubEntityBuilders?.Select(x => x.Build()).ToArray(),
                Links = LinkBuilders?.Select(x => x.Build()).ToArray(),
                Actions = ActionBuilders?.Select(x => x.Build()).ToArray(),
                Title = Title
            };

            if (subEntity.Actions != null && new HashSet<string>(subEntity.Actions.Select(x => x.Name)).Count != subEntity.Actions.Count)
                throw new ArgumentException("Action names MUST be unique within the set of actions for an entity.");

            return (TEntity) subEntity;
        }
    }
}
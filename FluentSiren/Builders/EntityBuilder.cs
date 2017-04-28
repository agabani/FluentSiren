using System;
using System.Collections.Generic;
using System.Linq;
using FluentSiren.Models;

namespace FluentSiren.Builders
{
    public class EntityBuilder : EntityBuilder<EntityBuilder, Entity>
    {
    }

    public class EntityBuilder<TBuilder, TEntity> : Builder<TBuilder, TEntity>
        where TBuilder : EntityBuilder<TBuilder, TEntity>
        where TEntity : Entity
    {
        protected List<string> Class;
        protected string Title;
        protected Dictionary<string, object> Properties;
        protected List<ISubEntityBuilder> SubEntityBuilders;
        protected List<LinkBuilder> LinkBuilders;
        protected List<ActionBuilder> ActionBuilders;

        public TBuilder WithClass(string @class)
        {
            if (Class == null)
                Class = new List<string>();

            Class.Add(@class);
            return This;
        }

        public TBuilder WithTitle(string title)
        {
            Title = title;
            return This;
        }

        public TBuilder WithProperty(string key, object value)
        {
            if (Properties == null)
                Properties = new Dictionary<string, object>();

            Properties[key] = value;
            return This;
        }

        public TBuilder WithSubEntity(ISubEntityBuilder subEntityBuilder)
        {
            if (SubEntityBuilders == null)
                SubEntityBuilders = new List<ISubEntityBuilder>();

            SubEntityBuilders.Add(subEntityBuilder);
            return This;
        }

        public TBuilder WithLink(LinkBuilder linkBuilder)
        {
            if (LinkBuilders == null)
                LinkBuilders = new List<LinkBuilder>();

            LinkBuilders.Add(linkBuilder);
            return This;
        }

        public TBuilder WithAction(ActionBuilder actionBuilder)
        {
            if (ActionBuilders == null)
                ActionBuilders = new List<ActionBuilder>();

            ActionBuilders.Add(actionBuilder);
            return This;
        }

        public override TEntity Build()
        {
            var entity = new Entity
            {
                Class = Class?.ToArray(),
                Title = Title,
                Properties = Properties?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
                Entities = SubEntityBuilders?.Select(x => x.Build()).ToArray(),
                Links = LinkBuilders?.Select(x => x.Build()).ToArray(),
                Actions = ActionBuilders?.Select(x => x.Build()).ToArray()
            };

            if (entity.Actions != null && new HashSet<string>(entity.Actions.Select(x => x.Name)).Count != entity.Actions.Count)
                throw new ArgumentException("Action names MUST be unique within the set of actions for an entity.");

            return (TEntity) entity;
        }
    }
}
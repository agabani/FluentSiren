using System;
using System.Collections.Generic;
using System.Linq;
using FluentSiren.Models;

namespace FluentSiren.Builders
{
    public class EmbeddedRepresentationBuilder : SubEntityBuilder
    {
        private List<ActionBuilder> _actionBuilders;
        private List<string> _class;
        private List<string> _rel;
        private List<LinkBuilder> _linkBuilders;
        private Dictionary<string, dynamic> _properties;
        private List<SubEntityBuilder> _subEntityBuilders;
        private string _title;

        public EmbeddedRepresentationBuilder WithClass(string @class)
        {
            if (_class == null)
                _class = new List<string>();

            _class.Add(@class);
            return this;
        }

        public EmbeddedRepresentationBuilder WithRel(string rel)
        {
            if (_rel == null)
                _rel = new List<string>();

            _rel.Add(rel);
            return this;
        }

        public EmbeddedRepresentationBuilder WithProperty(string key, dynamic value)
        {
            if (_properties == null)
                _properties = new Dictionary<string, dynamic>();

            _properties[key] = value;
            return this;
        }

        public EmbeddedRepresentationBuilder WithSubEntity(SubEntityBuilder subEntityBuilder)
        {
            if (_subEntityBuilders == null)
                _subEntityBuilders = new List<SubEntityBuilder>();

            _subEntityBuilders.Add(subEntityBuilder);
            return this;
        }

        public EmbeddedRepresentationBuilder WithLink(LinkBuilder linkBuilder)
        {
            if (_linkBuilders == null)
                _linkBuilders = new List<LinkBuilder>();

            _linkBuilders.Add(linkBuilder);
            return this;
        }

        public EmbeddedRepresentationBuilder WithAction(ActionBuilder actionBuilder)
        {
            if (_actionBuilders == null)
                _actionBuilders = new List<ActionBuilder>();

            _actionBuilders.Add(actionBuilder);
            return this;
        }

        public EmbeddedRepresentationBuilder WithTitle(string title)
        {
            _title = title;
            return this;
        }

        public override SubEntity Build()
        {
            if (_rel == null || !_rel.Any())
                throw new ArgumentException("Rel is required.");

            var subEntity = new SubEntity
            {
                Class = _class?.ToArray(),
                Rel = _rel?.ToArray(),
                Properties = _properties?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
                Entities = _subEntityBuilders?.Select(x => x.Build()).ToArray(),
                Links = _linkBuilders?.Select(x => x.Build()).ToArray(),
                Actions = _actionBuilders?.Select(x => x.Build()).ToArray(),
                Title = _title
            };

            if (subEntity.Actions != null && new HashSet<string>(subEntity.Actions.Select(x => x.Name)).Count != subEntity.Actions.Count)
                throw new ArgumentException("Action names MUST be unique within the set of actions for an entity.");

            return subEntity;
        }
    }
}
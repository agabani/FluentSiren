﻿using System.Collections.Generic;
using System.Linq;
using FluentSiren.Models;

namespace FluentSiren.Builders
{
    public class EntityBuilder
    {
        private List<ActionBuilder> _actionBuilders;
        private List<string> _class;
        private List<LinkBuilder> _linkBuilders;
        private Dictionary<string, dynamic> _properties;
        private List<SubEntityBuilder> _subEntityBuilders;
        private string _title;

        public EntityBuilder WithClass(string @class)
        {
            if (_class == null)
                _class = new List<string>();

            _class.Add(@class);
            return this;
        }

        public EntityBuilder WithProperty(string key, dynamic value)
        {
            if (_properties == null)
                _properties = new Dictionary<string, dynamic>();

            _properties[key] = value;
            return this;
        }

        public EntityBuilder WithSubEntity(SubEntityBuilder subEntityBuilder)
        {
            if (_subEntityBuilders == null)
                _subEntityBuilders = new List<SubEntityBuilder>();

            _subEntityBuilders.Add(subEntityBuilder);
            return this;
        }

        public EntityBuilder WithLink(LinkBuilder linkBuilder)
        {
            if (_linkBuilders == null)
                _linkBuilders = new List<LinkBuilder>();

            _linkBuilders.Add(linkBuilder);
            return this;
        }

        public EntityBuilder WithAction(ActionBuilder actionBuilder)
        {
            if (_actionBuilders == null)
                _actionBuilders = new List<ActionBuilder>();

            _actionBuilders.Add(actionBuilder);
            return this;
        }

        public EntityBuilder WithTitle(string title)
        {
            _title = title;
            return this;
        }

        public Entity Build()
        {
            return new Entity
            {
                Class = _class?.ToArray(),
                Properties = _properties?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
                Entities = _subEntityBuilders?.Select(x => x.Build()).ToArray(),
                Links = _linkBuilders?.Select(x => x.Build()).ToArray(),
                Actions = _actionBuilders?.Select(x => x.Build()).ToArray(),
                Title = _title
            };
        }
    }
}
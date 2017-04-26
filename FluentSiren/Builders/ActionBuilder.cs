using System;
using System.Collections.Generic;
using System.Linq;
using Action = FluentSiren.Models.Action;

namespace FluentSiren.Builders
{
    public class ActionBuilder : ActionBuilder<ActionBuilder, Action>
    {
    }

    public class ActionBuilder<TBuilder, TEntity> : Builder<TBuilder, TEntity>
        where TBuilder : ActionBuilder<TBuilder, TEntity>
        where TEntity : Action
    {
        private string _name;
        private List<string> _class;
        private string _method;
        private string _href;
        private string _title;
        private string _type;
        private List<FieldBuilder> _fieldBuilders;

        public TBuilder WithName(string name)
        {
            _name = name;
            return This;
        }

        public TBuilder WithClass(string @class)
        {
            if (_class == null)
                _class = new List<string>();

            _class.Add(@class);
            return This;
        }

        public TBuilder WithMethod(string method)
        {
            _method = method;
            return This;
        }

        public TBuilder WithHref(string href)
        {
            _href = href;
            return This;
        }

        public TBuilder WithTitle(string title)
        {
            _title = title;
            return This;
        }

        public TBuilder WithType(string type)
        {
            _type = type;
            return This;
        }

        public TBuilder WithField(FieldBuilder fieldBuilder)
        {
            if (_fieldBuilders == null)
                _fieldBuilders = new List<FieldBuilder>();

            _fieldBuilders.Add(fieldBuilder);
            return This;
        }

        public override TEntity Build()
        {
            if (string.IsNullOrEmpty(_name))
                throw new ArgumentException("Name is required.");

            if (string.IsNullOrEmpty(_href))
                throw new ArgumentException("Href is required.");

            var action = new Action
            {
                Name = _name,
                Class = _class?.ToArray(),
                Method = !string.IsNullOrEmpty(_method) ? _method : "GET",
                Href = _href,
                Title = _title,
                Type = !string.IsNullOrEmpty(_type) ? _type : _fieldBuilders != null ? "application/x-www-form-urlencoded" : null,
                Fields = _fieldBuilders?.Select(x => x.Build()).ToArray()
            };

            if (action.Fields != null && new HashSet<string>(action.Fields.Select(x => x.Name)).Count != action.Fields.Count)
                throw new ArgumentException("Field names MUST be unique within the set of fields for an action.");

            return (TEntity) action;
        }
    }
}
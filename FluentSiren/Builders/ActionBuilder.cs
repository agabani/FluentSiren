using System;
using System.Collections.Generic;
using System.Linq;
using Action = FluentSiren.Models.Action;

namespace FluentSiren.Builders
{
    public class ActionBuilder
    {
        private List<string> _class;
        private List<FieldBuilder> _fieldBuilders;
        private string _href;
        private string _method;
        private string _name;
        private string _title;
        private string _type;

        public ActionBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public ActionBuilder WithClass(string @class)
        {
            if (_class == null)
                _class = new List<string>();

            _class.Add(@class);
            return this;
        }

        public ActionBuilder WithMethod(string method)
        {
            _method = method;
            return this;
        }

        public ActionBuilder WithHref(string href)
        {
            _href = href;
            return this;
        }

        public ActionBuilder WithTitle(string title)
        {
            _title = title;
            return this;
        }

        public ActionBuilder WithType(string type)
        {
            _type = type;
            return this;
        }

        public ActionBuilder WithField(FieldBuilder fieldBuilder)
        {
            if (_fieldBuilders == null)
                _fieldBuilders = new List<FieldBuilder>();

            _fieldBuilders.Add(fieldBuilder);
            return this;
        }

        public Action Build()
        {
            if (string.IsNullOrEmpty(_name))
                throw new ArgumentException("Name is required");

            if (string.IsNullOrEmpty(_href))
                throw new ArgumentException("Href is required");

            var action = new Action
            {
                Name = _name,
                Class = _class?.ToArray(),
                Method = !string.IsNullOrEmpty(_method) ? _method : "GET",
                Href = _href,
                Title = _title,
                Type = !string.IsNullOrEmpty(_type) ? _type : "application/x-www-form-urlencoded",
                Fields = _fieldBuilders?.Select(x => x.Build()).ToArray()
            };

            if (new HashSet<string>(action.Fields.Select(x => x.Name)).Count != action.Fields.Count)
                throw new ArgumentException("Field names MUST be unique within the set of fields for an action.");

            return action;
        }
    }
}
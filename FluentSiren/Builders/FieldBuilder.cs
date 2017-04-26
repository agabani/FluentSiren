using System;
using System.Collections.Generic;
using FluentSiren.Models;

namespace FluentSiren.Builders
{
    public class FieldBuilder
    {
        private List<string> _class;
        private string _name;
        private string _title;
        private string _type;
        private object _value;

        public FieldBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public FieldBuilder WithClass(string @class)
        {
            if (_class == null)
                _class = new List<string>();

            _class.Add(@class);
            return this;
        }

        public FieldBuilder WithType(string type)
        {
            _type = type;
            return this;
        }

        public FieldBuilder WithValue(object value)
        {
            _value = value;
            return this;
        }

        public FieldBuilder WithTitle(string title)
        {
            _title = title;
            return this;
        }

        public Field Build()
        {
            if (string.IsNullOrEmpty(_name))
                throw new ArgumentException("Name is required.");

            if (_value != null && (!(_value is ValueType || _value is string) || _value is bool))
                throw new ArgumentException("Value must be a string or a number.");

            return new Field
            {
                Name = _name,
                Class = _class?.ToArray(),
                Type = !string.IsNullOrEmpty(_type) ? _type : "text",
                Value = _value,
                Title = _title
            };
        }
    }
}
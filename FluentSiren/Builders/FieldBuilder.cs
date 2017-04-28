using System;
using System.Collections.Generic;
using FluentSiren.Models;

namespace FluentSiren.Builders
{
    public class FieldBuilder : FieldBuilder<FieldBuilder, Field>
    {
    }

    public class FieldBuilder<TBuilder, TEntity> : Builder<TBuilder, TEntity>
        where TBuilder : FieldBuilder<TBuilder, TEntity>
        where TEntity : Field
    {
        private string _name;
        private List<string> _class;
        private string _type;
        private object _value;
        private string _title;

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

        public TBuilder WithType(string type)
        {
            _type = type;
            return This;
        }

        public TBuilder WithValue(object value)
        {
            _value = value;
            return This;
        }

        public TBuilder WithTitle(string title)
        {
            _title = title;
            return This;
        }

        public override TEntity Build()
        {
            if (string.IsNullOrEmpty(_name))
                throw new ArgumentException("Name is required.");

            if (_value != null && (!(_value is ValueType || _value is string) || _value is bool))
                throw new ArgumentException("Value must be a string or a number.");

            return (TEntity) new Field
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
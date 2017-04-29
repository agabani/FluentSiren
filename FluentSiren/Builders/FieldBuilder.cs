using System;
using System.Collections.Generic;
using System.Linq;
using FluentSiren.Enums;
using FluentSiren.Models;
using Type = FluentSiren.Enums.Type;

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
        private Type? _type;
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

        public TBuilder WithType(Type type)
        {
            _type = type;
            return This;
        }

        public TBuilder WithValue(object value)
        {
            _value = value;
            return This;
        }

        public TBuilder WithValue(FieldValueBuilder fieldValueBuilder)
        {
            if (!(_value is List<FieldValueBuilder>))
                _value = new List<FieldValueBuilder>();

            ((List<FieldValueBuilder>) _value).Add(fieldValueBuilder);
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

            if (_value != null && (!(_value is ValueType || _value is string || _value is List<FieldValueBuilder>) || _value is bool))
                throw new ArgumentException("Value must be a string, a number, or a list of field values.");

            return (TEntity) new Field
            {
                Name = _name,
                Class = _class?.ToArray(),
                Type = _type.GetName() ?? Type.Text.GetName(),
                Value = _value is List<FieldValueBuilder> ? ((List<FieldValueBuilder>) _value).Select(x => x.Build()).ToArray() : _value,
                Title = _title
            };
        }
    }
}
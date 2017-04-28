using System;
using FluentSiren.Models;

namespace FluentSiren.Builders
{
    public class FieldValueBuilder : FieldValueBuilder<FieldValueBuilder, FieldValue>
    {
    }

    public class FieldValueBuilder<TBuilder, TEntity> : Builder<TBuilder, TEntity>
        where TBuilder : FieldValueBuilder<TBuilder, TEntity>
        where TEntity : FieldValue
    {
        private string _title;
        private object _value;
        private bool _selected;

        public TBuilder WithTitle(string title)
        {
            _title = title;
            return This;
        }

        public TBuilder WithValue(object value)
        {
            _value = value;
            return This;
        }

        public TBuilder WithSelected(bool selected)
        {
            _selected = selected;
            return This;
        }

        public override TEntity Build()
        {
            if (_value == null)
                throw new ArgumentException("Value is required.");

            if (_value != null && (!(_value is ValueType || _value is string) || _value is bool))
                throw new ArgumentException("Value must be a string or a number.");


            return (TEntity) new FieldValue
            {
                Title = _title,
                Value = _value,
                Selected = _selected
            };
        }
    }
}
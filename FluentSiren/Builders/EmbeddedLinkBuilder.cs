using System;
using System.Collections.Generic;
using System.Linq;
using FluentSiren.Models;

namespace FluentSiren.Builders
{
    public class EmbeddedLinkBuilder : EmbeddedLinkBuilder<EmbeddedLinkBuilder, Entity>, ISubEntityBuilder
    {
    }

    public class EmbeddedLinkBuilder<TBuilder, TEntity> : Builder<TBuilder, TEntity>
        where TBuilder : EmbeddedLinkBuilder<TBuilder, TEntity>
        where TEntity : Entity
    {
        private List<string> _class;
        private List<string> _rel;
        private string _href;
        private string _type;
        private string _title;

        public TBuilder WithClass(string @class)
        {
            if (_class == null)
                _class = new List<string>();

            _class.Add(@class);
            return This;
        }

        public TBuilder WithRel(string rel)
        {
            if (_rel == null)
                _rel = new List<string>();

            _rel.Add(rel);
            return This;
        }

        public TBuilder WithHref(string href)
        {
            _href = href;
            return This;
        }

        public TBuilder WithType(string type)
        {
            _type = type;
            return This;
        }

        public TBuilder WithTitle(string title)
        {
            _title = title;
            return This;
        }

        public override TEntity Build()
        {
            if (_rel == null || !_rel.Any())
                throw new ArgumentException("Rel is required.");

            if (string.IsNullOrEmpty(_href))
                throw new ArgumentException("Href is required.");

            return (TEntity) new Entity
            {
                Class = _class?.ToArray(),
                Rel = _rel?.ToArray(),
                Href = _href,
                Type = _type,
                Title = _title
            };
        }
    }
}
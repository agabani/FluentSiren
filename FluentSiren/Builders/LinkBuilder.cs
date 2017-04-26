using System;
using System.Collections.Generic;
using System.Linq;
using FluentSiren.Models;

namespace FluentSiren.Builders
{
    public class LinkBuilder : LinkBuilder<LinkBuilder, Link>
    {
    }

    public class LinkBuilder<TBuilder, TEntity> : Builder<TBuilder, TEntity>
        where TBuilder : LinkBuilder<TBuilder, TEntity>
        where TEntity : Link
    {
        private readonly List<string> _rel = new List<string>();
        private List<string> _class;
        private string _href;
        private string _title;
        private string _type;

        public TBuilder WithRel(string rel)
        {
            _rel.Add(rel);
            return This;
        }

        public TBuilder WithClass(string @class)
        {
            if (_class == null)
                _class = new List<string>();

            _class.Add(@class);
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

        public override TEntity Build()
        {
            if (!_rel.Any())
                throw new ArgumentException("Rel is required.");

            if (string.IsNullOrEmpty(_href))
            {
                throw new ArgumentException("Href is required.");
            }

            return (TEntity) new Link
            {
                Rel = _rel.ToArray(),
                Class = _class?.ToArray(),
                Href = _href,
                Title = _title,
                Type = _type
            };
        }
    }
}
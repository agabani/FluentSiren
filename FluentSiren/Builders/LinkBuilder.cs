using System;
using System.Collections.Generic;
using System.Linq;
using FluentSiren.Enums;
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
        private readonly List<Rel> _rel = new List<Rel>();
        private List<string> _class;
        private Uri _href;
        private string _title;
        private string _type;

        public TBuilder WithRel(Rel rel)
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

        public TBuilder WithHref(Uri href)
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

            if (_href == null)
            {
                throw new ArgumentException("Href is required.");
            }

            return (TEntity) new Link
            {
                Rel = _rel.Select(x => x.GetName()).ToArray(),
                Class = _class?.ToArray(),
                Href = _href.ToString(),
                Title = _title,
                Type = _type
            };
        }
    }
}
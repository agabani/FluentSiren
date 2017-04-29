using System;
using System.Collections.Generic;
using System.Linq;
using FluentSiren.Enums;
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
        private readonly List<object> _rel = new List<object>();
        private Uri _href;
        private string _type;
        private string _title;

        public TBuilder WithClass(string @class)
        {
            if (_class == null)
                _class = new List<string>();

            _class.Add(@class);
            return This;
        }

        public TBuilder WithRel(Uri rel)
        {
            _rel.Add(rel);
            return This;
        }

        public TBuilder WithRel(Rel rel)
        {
            _rel.Add(rel);
            return This;
        }

        public TBuilder WithHref(Uri href)
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
            if (!_rel.Any())
                throw new ArgumentException("Rel is required.");

            if (_href == null)
                throw new ArgumentException("Href is required.");

            return (TEntity) new Entity
            {
                Class = _class?.ToArray(),
                Rel = _rel.Select(x => x is Rel ? ((Rel) x).GetName() : ((Uri) x).ToString()).ToArray(),
                Href = _href.ToString(),
                Type = _type,
                Title = _title
            };
        }
    }
}
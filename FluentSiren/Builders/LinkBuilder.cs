using System;
using System.Collections.Generic;
using System.Linq;
using FluentSiren.Models;

namespace FluentSiren.Builders
{
    public class LinkBuilder
    {
        private readonly List<string> _rel = new List<string>();
        private List<string> _class;
        private string _href;
        private string _title;
        private string _type;

        public LinkBuilder WithRel(string rel)
        {
            _rel.Add(rel);
            return this;
        }

        public LinkBuilder WithClass(string @class)
        {
            if (_class == null)
                _class = new List<string>();

            _class.Add(@class);
            return this;
        }

        public LinkBuilder WithHref(string href)
        {
            _href = href;
            return this;
        }

        public LinkBuilder WithTitle(string title)
        {
            _title = title;
            return this;
        }

        public LinkBuilder WithType(string type)
        {
            _type = type;
            return this;
        }

        public Link Build()
        {
            if (!_rel.Any())
                throw new ArgumentException("Rel is required.");

            if (string.IsNullOrEmpty(_href))
            {
                throw new ArgumentException("Href is required.");
            }

            return new Link
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
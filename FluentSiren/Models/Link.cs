using System.Collections.Generic;

namespace FluentSiren.Models
{
    public class Link
    {
        internal Link()
        {
        }

        public IReadOnlyList<string> Rel { get; internal set; }
        public IReadOnlyList<string> Class { get; internal set; }
        public string Href { get; internal set; }
        public string Title { get; internal set; }
        public string Type { get; internal set; }
    }
}
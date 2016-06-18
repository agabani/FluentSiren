using System.Collections.Generic;

namespace FluentSiren.Models
{
    public class Action
    {
        internal Action()
        {
        }

        public string Name { get; internal set; }
        public IReadOnlyList<string> Class { get; internal set; }
        public string Method { get; internal set; }
        public string Href { get; internal set; }
        public string Title { get; internal set; }
        public string Type { get; internal set; }
        public IReadOnlyList<Field> Fields { get; internal set; }
    }
}
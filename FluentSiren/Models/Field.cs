using System.Collections.Generic;

namespace FluentSiren.Models
{
    public class Field
    {
        internal Field()
        {
        }

        public string Name { get; internal set; }
        public IReadOnlyList<string> Class { get; internal set; }
        public string Type { get; internal set; }
        public object Value { get; internal set; }
        public string Title { get; internal set; }
    }
}
using System.Collections.Generic;

namespace FluentSiren.Models
{
    public class SubEntity
    {
        internal SubEntity()
        {
        }

        public IReadOnlyList<string> Class { get; internal set; }
        public IReadOnlyList<string> Rel { get; internal set; }
        public IReadOnlyDictionary<string, dynamic> Properties { get; internal set; }
        public IReadOnlyList<SubEntity> Entities { get; internal set; }
        public IReadOnlyList<Link> Links { get; internal set; }
        public IReadOnlyList<Action> Actions { get; internal set; }
        public string Href { get; internal set; }
        public string Type { get; internal set; }
        public string Title { get; internal set; }
    }
}
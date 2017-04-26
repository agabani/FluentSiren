using System.Collections.Generic;

namespace FluentSiren.Models
{
    public class SubEntity : Entity
    {
        internal SubEntity()
        {
        }

        public IReadOnlyList<string> Rel { get; internal set; }
        public string Href { get; internal set; }
        public string Type { get; internal set; }
    }
}
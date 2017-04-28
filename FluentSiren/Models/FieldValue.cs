namespace FluentSiren.Models
{
    public class FieldValue
    {
        internal FieldValue()
        {
        }

        public string Title { get; internal set; }
        public object Value { get; internal set; }
        public bool Selected { get; internal set; }
    }
}
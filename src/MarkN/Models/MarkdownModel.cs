namespace MarkN.Models
{
    public class MarkdownModel
    {
        private bool _sanitize = true;

        public string Text { get; set; }

        public bool Sanitize
        {
            get { return _sanitize; }
            set { _sanitize = value; }
        }
    }
}
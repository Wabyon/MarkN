using System.Collections.Generic;
using Nancy.ViewEngines.Razor;

namespace MarkN
{
    public class RazorConfig : IRazorConfiguration
    {
        public IEnumerable<string> GetAssemblyNames()
        {
            yield return "MarkN";
        }

        public IEnumerable<string> GetDefaultNamespaces()
        {
            yield return "MarkN";
            yield return "MarkN.Razor";
            yield return "MarkN.Models";
        }

        public bool AutoIncludeModelNamespace
        {
            get { return true; }
        }
    }
}
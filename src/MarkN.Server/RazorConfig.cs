using System.Collections.Generic;
using Nancy.ViewEngines.Razor;

namespace MarkN
{
    public class RazorConfig : IRazorConfiguration
    {
        public IEnumerable<string> GetAssemblyNames()
        {
            yield return "MarkN.Server";
        }

        public IEnumerable<string> GetDefaultNamespaces()
        {
            yield return "MarkN";
        }

        public bool AutoIncludeModelNamespace
        {
            get { return true; }
        }
    }
}
using System;
using Nancy;

namespace MarkN.SelfHost
{
    public class SelfHostPathProvider : IRootPathProvider
    {
        public string GetRootPath()
        {
#if DEBUG
            return new Uri(new Uri(AppDomain.CurrentDomain.BaseDirectory), @"..\..\..\MarkN").AbsolutePath;
#endif
            return AppDomain.CurrentDomain.BaseDirectory;
        }
    }
}

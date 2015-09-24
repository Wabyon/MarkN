using System;
using System.IO;
using Nancy;

namespace MarkN
{
    public class AspNetPathProvider : IRootPathProvider
    {
        public string GetRootPath()
        {
#if DEBUG
            return new Uri(new Uri(AppDomain.CurrentDomain.BaseDirectory), @"..\MarkN").AbsolutePath;
#endif
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"bin");
        }
    }
}

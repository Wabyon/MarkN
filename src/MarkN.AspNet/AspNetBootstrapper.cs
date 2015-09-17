using Nancy;

namespace MarkN
{
    public class AspNetBootstrapper : NancyBootstrapper
    {
        protected override IRootPathProvider RootPathProvider
        {
            get { return new AspNetPathProvider(); }
        }
    }
}
using Nancy;

namespace MarkN.SelfHost
{
    public class SelfHostBootstrapper : NancyBootstrapper
    {
        protected override IRootPathProvider RootPathProvider
        {
            get { return new SelfHostPathProvider(); }
        }
    }
}
using JavaScriptEngineSwitcher.V8;
using MarkN.Models;
using Nancy;
using Nancy.ModelBinding;

namespace MarkN.Modules
{
    public class MarkdownModule : NancyModule
    {
        public MarkdownModule()
        {
            Post["/markdown"] = _ =>
            {
                MarkdownModel model;
                try
                {
                    model = this.Bind<MarkdownModel>();
                }
                catch
                {
                    return HttpStatusCode.BadRequest;
                }

                if (model.Text == null) return HttpStatusCode.BadRequest;

                using (var jsEngine = new V8JsEngine())
                using (var markdown = new Markdown(jsEngine))
                {
                    var html = markdown.Transform(model.Text, model.Sanitize);
                    return Response.AsText(html,"text/html");
                }
            };
        }
    }
}
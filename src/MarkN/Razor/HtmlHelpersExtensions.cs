using JavaScriptEngineSwitcher.V8;
using Nancy.ViewEngines.Razor;

namespace MarkN.Razor
{
    public static class HtmlHelpersExtensions
    {
        public static IHtmlString Markdown<T>(this HtmlHelpers<T> helpers, string text, bool sanitize = true)
        {
            using (var jsEngine = new V8JsEngine())
            using (var markdown = new Markdown(jsEngine))
            {
                return new NonEncodedHtmlString(markdown.Transform(text, sanitize));
            }
        }
    }
}
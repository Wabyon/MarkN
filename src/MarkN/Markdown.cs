using System;
using JavaScriptEngineSwitcher.Core;

namespace MarkN
{
    public class Markdown : IDisposable
    {
        private readonly object _compilationSynchronizer = new object();

        private IJsEngine _jsEngine;
        private bool _initialized;
        private bool _disposed;

        public Markdown(IJsEngine jsEngine)
        {
            if (jsEngine == null) throw new ArgumentNullException("jsEngine");
            _jsEngine = jsEngine;
        }

        private void Initialize()
        {
            if (_initialized) return;

            var type = GetType();
            
            _jsEngine.Execute("this.window = this;");
            _jsEngine.ExecuteResource("MarkN.Scripts.marked.min.js", type);
            _jsEngine.ExecuteResource("MarkN.Scripts.highlight.pack.js", type);

            _initialized = true;
        }

        public string Transform(string text, bool sanitize = true)
        {
            if (text == null) throw new ArgumentNullException("text");

            string result;

            lock (_compilationSynchronizer)
            {
                Initialize();

                _jsEngine.Evaluate(@"
marked.setOptions({
    gfm: true,
    tables: true,
    breaks: true,
    pedantic: false,
    sanitize: false,
    smartLists: true,
    silent: false,
    highlight: function (code) {
        return hljs.highlightAuto(code).value;
    },
    langPrefix: '',
    smartypants: false,
    headerPrefix: '',
    renderer: new marked.Renderer(),
    xhtml: false
});");

                result = _jsEngine.CallFunction<string>("marked", text);
            }

            return (sanitize) ? HtmlUtility.SanitizeHtml(result) : result;
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            if (_jsEngine == null) return;
            _jsEngine.Dispose();

            _jsEngine = null;
        }
    }
}

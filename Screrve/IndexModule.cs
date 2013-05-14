namespace Screrve
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Web.Hosting;
    using Nancy;

    public class IndexModule : NancyModule
    {
        private const string jsExt = "*.js";
        private static readonly FileStorage _store = new FileStorage();

        public IndexModule()
        {
            Get["/"] = parameters =>
            {
                return View["index"];
            };
            Get["scripts"] = parameters =>
            {
                // can't catch in a lambda :(
                return SafeView(() => View["scripts", GetJavascriptFiles()]);
            };
            Get["access"] = parameters =>
            {
                return View["access"];
            };
            Post["/"] = parameters =>
            {
                return SafeView(() =>
                {
                    _store.AddFiles(Context.Request.Files.Where(x => x.Name.EndsWith(".js")));
                    return View["scripts", GetJavascriptFiles()];
                });
            };
            Delete["scripts"] = parameters =>
            {
                return Remove();
            };
            Delete["/"] = parameters =>
            {
                return Remove();
            };
        }

        /// <summary>
        /// Safes the view.
        /// </summary>
        /// <returns></returns>
        private object SafeView(Func<object> view)
        {
            try
            {
                return view();
            }
            catch(Exception ex)
            {
                return ex.ToString();
            }
        }
        private dynamic Remove()
        {
            var filename = Request.Query.Remove;
            _store.RemoveFile(filename);
            return View["scripts", GetJavascriptFiles()];
        }

        private FileDescription[] GetJavascriptFiles()
        {
            return _store.GetFiles().ToArray();
        }
    }
}
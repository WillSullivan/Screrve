namespace Screrve
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Web;
    using Nancy;

    public class IndexModule : NancyModule
    {
        private const string jsExt = "*.js";
        public IndexModule()
        {
            Get["/"] = parameters =>
            {
                return View["index"];
            };
            Get["scripts"] = parameters =>
            {
                return View["scripts", GetJavascriptFiles()];
            };
            Get["access"] = parameters =>
            {
                return View["access"];
            };
            Post["/"] = parameters =>
            {
                var dir = GetAppDataFolder();
                SaveNewFiles(dir);
                return View["scripts", GetJavascriptFiles(dir)];
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

        private dynamic Remove()
        {
            var filename = Request.Query.Remove;
            var dir = GetAppDataFolder();
            filename = Path.Combine(dir.FullName, Path.ChangeExtension(filename, "js"));
            if(File.Exists(filename))
                File.Delete(filename);
            return View["scripts", GetJavascriptFiles()];
        }
        private FileInfo[] GetJavascriptFiles()
        {
            return GetJavascriptFiles(GetAppDataFolder());
        }

        private FileInfo[] GetJavascriptFiles(DirectoryInfo dir)
        {
            FileInfo[] resultingFiles;
            resultingFiles = dir.EnumerateFiles(jsExt).ToArray();
            return resultingFiles;
        }

        private DirectoryInfo GetAppDataFolder()
        {
            return new DirectoryInfo(global::System.Web.HttpContext.Current.Server.MapPath("~/App_Data"));
        }

        private void SaveNewFiles(DirectoryInfo dir)
        {
            if(!this.Context.Request.Files.Any())
                return;

            var existingFiles = dir.EnumerateFiles(jsExt);
            var files = Context.Request.Files.Where(x => x.Name.EndsWith(".js") && !existingFiles.Any(y => y.Name.Equals(x.Name, StringComparison.CurrentCultureIgnoreCase)));
            foreach(var file in files)
                using(var stream = File.OpenWrite(Path.Combine(dir.FullName, file.Name)))
                    file.Value.CopyTo(stream);
        }
    }
}
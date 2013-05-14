using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;

namespace Screrve
{
    public sealed class FileStorage
    {
        public IEnumerable<FileDescription> GetFiles()
        {
            //CloudConfigurationManager.GetSetting("CLOUD_STORAGE_ACCOUNT");

            var connex = ConfigurationManager.ConnectionStrings["CLOUD_STORAGE_ACCOUNT"];

            var derp = string.Join(", ", ConfigurationManager.ConnectionStrings.OfType<ConnectionStringSettings>().Select(x => x.Name));

            //var storageAccount = CloudStorageAccount.Parse(connex.ConnectionString);

            throw new NotImplementedException(connex == null ? derp : connex.ConnectionString);
        }

        internal void RemoveFile(dynamic filename)
        {
            throw new NotImplementedException();
        }

        internal void AddFiles(IEnumerable<Nancy.HttpFile> enumerable)
        {
            throw new NotImplementedException();
            // && !existingFiles.Any(y => y.Name.Equals(x.Name, StringComparison.CurrentCultureIgnoreCase)))
        }
    }

    public sealed class FileDescription
    {
        public string Name { get; set; }
        public string Content { get; set; }
    }
}
using System;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace Primavera.Extensibility.Core.Helper
{
    public class VsixManifest
    {
        private string _manifestPath = string.Empty;

        public VsixManifest()
        {
            string manifestPath = System.IO.Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().Location);

            _manifestPath = System.IO.Path.Combine(manifestPath, "extension.vsixmanifest");
        }

        public string GetVsixManifestVersion()
        {
            try
            {
                var doc = new XmlDocument();
                doc.Load(_manifestPath);

                if (doc.DocumentElement == null || doc.DocumentElement.Name != "PackageManifest")
                    return null;
                else
                {
                    var metaData = doc.DocumentElement.ChildNodes.Cast<XmlElement>().First(x => x.Name == "Metadata");
                    var identity = metaData.ChildNodes.Cast<XmlElement>().First(x => x.Name == "Identity");

                    return identity.GetAttribute("Version");
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }
    }
}

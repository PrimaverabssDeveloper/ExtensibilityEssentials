using EnvDTE;
using Primavera.Extensibility.Options;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VSLangProj;

namespace Primavera.Extensibility
{
    internal static class WizardHelper
    {
        #region constants

        private const string MajorVersion = "100";

        #endregion

        #region Properties

        private static string Module { get; set; }
        private static string ModuleType { get; set; }
        private static string ClassName { get; set; }

        #endregion

        #region Public methods

        public static void AddBaseReferences(Project project, string type, string filePath)
        {
            VSProject vsProj = (VSProject)project.Object;

            List<string> result = DependenciesDictionary.BaseDependencies.Where(el => el.Key.StartsWith(type)).Select(p => p.Value).ToList();

            foreach(string item in result)
            {
                vsProj.References.Add(GetFullPath(item.Replace("XXX", MajorVersion), filePath));
                vsProj.References.Item(item.Replace("XXX", MajorVersion)).CopyLocal = false;
            }
        }

        public static void AddModuleReference(Project project, string assemblyName)
        {
            VSProject vsProj = (VSProject)project.Object;
 
            vsProj.References.Add(GetFullPath(assemblyName, GeneralOptions.Instance.Path));
            vsProj.References.Item(assemblyName).CopyLocal = false;  
        }

        public static void AddDependenciesReference(Project project, string module)
        {
            string result = string.Empty;

            DependenciesDictionary.Dependencies.TryGetValue(module, out result);

            if (result != null)
            {
                result = result.Replace("XXX", MajorVersion);
                string[] dependenciesParts = result.Split(';');

                for (int i = 0; i < dependenciesParts.Length; i++)
                {
                    AddModuleReference(project, dependenciesParts[i]);
                }
            }
        }

        #endregion

        #region Private Methods

        private static string GetFullPath(string assemblyName, string InstallFolder)
        {
            if (Directory.Exists(InstallFolder))
            {
                return System.IO.Path.Combine(InstallFolder, assemblyName + ".dll");
            }
            else
            {
                throw new DirectoryNotFoundException("Could not find directory: " + InstallFolder);
            }
        }

        #endregion
    }
}
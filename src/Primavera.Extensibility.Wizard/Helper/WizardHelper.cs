using EnvDTE;
using Primavera.Extensibility.Options;
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

        public static void AddGenericReference(Project project)
        {
            VSProject vsProj = (VSProject)project.Object;

            for (int i = 0; i < DependenciesDictionary.BaseDependencies.Count; i++)
            {
                string result = DependenciesDictionary.BaseDependencies[DependenciesDictionary.BaseDependencies.Keys.ElementAt(i)];
                result = result.Replace("XXX", MajorVersion);

                vsProj.References.Add(GetFullPath(result));
                vsProj.References.Item(result).CopyLocal = false;
            }
        }
        public static void AddModuleReference(Project project, string assemblyName)
        {
            VSProject vsProj = (VSProject)project.Object;
 
            vsProj.References.Add(GetFullPath(assemblyName));
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

        private static string GetFullPath(string assemblyName)
        {
            string InstallFolder = GeneralOptions.Instance.Path;

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
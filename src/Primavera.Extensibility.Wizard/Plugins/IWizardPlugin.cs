using EnvDTE;

namespace Primavera.Extensibility.Plugins
{
    internal interface IWizardPlugin
    {
        void FinishedGenerating(Project project);

        void ItemsFinishedGenerating(ProjectItem projectItem);

        void ItemFinishedGenerating(ProjectItem projectItem);

        void AddProjectItem(string filePath);
    }
}
using System.Collections.Generic;
using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;

namespace Primavera.Extensibility.Wizard
{
    public class PRIExtensibilityEx : IWizard
    {
        #region private objects
        private int i = 1;
        private string Name { get; set; }
        #endregion

        #region public methods
        // This method is called before opening any item that   
        // has the OpenInEditor attributes.  
        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        public void ProjectFinishedGenerating(Project project)
        {
        }

        // This method is only called for item templates,  
        // not for project templates.  
        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
            
            // Add the module reference..,
            if (this.Name == "PriCustomForm.cs" || this.Name == "PriCustomForm.vb")
            {
                // Add the module reference..,
                WizardHelper.AddModuleReference(projectItem.ContainingProject, "Primavera.Extensibility.CustomForm");
                //WizardHelper.AddModuleReference(projectItem.ContainingProject, "DevExpress.Utils.v18.1");
            }
            else
            {
                WizardHelper.AddModuleReference(projectItem.ContainingProject, "Primavera.Extensibility.CustomCode");
            }
        }

        // This method is called after the project is created.  
        public void RunFinished()
        {
        }
        public void RunStarted(object automationObject,
            Dictionary<string, string> replacementsDictionary,
            WizardRunKind runKind, object[] customParams)
        {
        }

        // This method is only called for item templates,  
        // not for project templates.   
        public bool ShouldAddProjectItem(string filePath)
        {
            this.Name = filePath;
            return true;
        }
        #endregion
    }
}
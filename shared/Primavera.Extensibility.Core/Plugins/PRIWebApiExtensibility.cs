using System;
using System.Collections.Generic;
using System.Windows.Forms;
using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using Primavera.Extensibility.Options;
using Primavera.Extensibility.Presentation;
using VSLangProj;

namespace Primavera.Extensibility.Wizard
{
    public class PRIWebApiExtensibility : IWizard
    {
        #region private objects

        private List<TreeNode> _selectedTypes = null;
        private string _controllerName;
        private EnvDTE.DTE _envDTE;

        #endregion

        #region Iwizard Interfaces

        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        public void ProjectFinishedGenerating(Project project)
        {
            if (_selectedTypes != null)
            {
                string _validationErrors = string.Empty;
                string _itemPath = null;
                string _fileExtension = null;

                EnvDTE80.Events2 _objEvents2;
                EnvDTE80.Solution2 _solution = project.DTE.Solution as EnvDTE80.Solution2;

                OutputWindowManager outPutWindowmng = new OutputWindowManager();

                // Subscribe all the IDE events
                _envDTE = project.DTE;
                _objEvents2 = ((EnvDTE80.Events2)(_envDTE.Events));

                // Write the post-build command to register the assembly.
                if (VSOptionsValidator.CanCreatePostBuildEvent(ref _validationErrors))
                {
                    // Add command-line parameters.
                    string command = $"copy /Y \"$(TargetPath)\" \"{WebApiOptions.Instance.Path}\\$(ProjectName).dll\"";

                    EnvDTE.Properties configmg = project.Properties;
                    configmg.Item("PostBuildEvent").Value = command;

                    outPutWindowmng.WriteMessage("The post-build command was added to the project.", OutputWindowMessagesType.Message);
                }
                else
                {
                    outPutWindowmng.WriteMessage("The post-build event has not been configured because of the following validation issues:", OutputWindowMessagesType.Warning);
                    outPutWindowmng.WriteMessage(_validationErrors, OutputWindowMessagesType.Message);
                    outPutWindowmng.WriteMessage("Check this on Tools | Options | PRIMAVERA Extensibility.", OutputWindowMessagesType.Message);
                }

                if (project.Kind == PrjKind.prjKindCSharpProject)
                {
                    _itemPath = _solution.GetProjectItemTemplate("PriWebApiControler.zip", "CSharp");
                    _fileExtension = ".cs";
                }
                else
                {
                    _itemPath = _solution.GetProjectItemTemplate("PriWebApiControler.zip", "VisualBasic");
                    _fileExtension = ".vb";
                }

                // Add folder to keep all controllers.
                ProjectItem rootFolder = project.ProjectItems.AddFolder("Controllers");

                // Add the class to the project.
                rootFolder.ProjectItems.AddFromTemplate(_itemPath, _controllerName + _fileExtension);

                // Add generic references
                //WizardHelper.AddApiBaseDependencies(project);
                WizardHelper.AddBaseReferences(project, "Bas", GeneralOptions.Instance.Path);
                WizardHelper.AddBaseReferences(project, "Api", WebApiOptions.Instance.Path);

                // Add references to the select modules
                foreach (TreeNode type in _selectedTypes)
                {
                    if (!String.IsNullOrEmpty(_itemPath))
                    {                    
                        // Add dependencies to the select modules.
                        WizardHelper.AddDependenciesReference(project, type.Text);
                    }
                }

                outPutWindowmng.WriteMessage("The project was created with success.");
            }
        }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {

        }

        public void RunFinished()
        {
        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            using (WebApiUI webApiUI = new WebApiUI())
            {
                if (webApiUI.ShowDialog() == DialogResult.OK)
                {
                    _controllerName = webApiUI.ControllerName.TrimEnd();
                    _selectedTypes = webApiUI.SelectedTypes;
                }
                else
                {
                    throw new WizardCancelledException();
                }
            }
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }
        #endregion
    }
}

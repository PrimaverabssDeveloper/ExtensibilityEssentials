using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using Primavera.Extensibility.Options;
using Primavera.Extensibility.Presentation;
using VSLangProj;

namespace Primavera.Extensibility.Wizard
{
    public class PRIExtensibility : IWizard
    {
        #region private properties

        private MyTreeNode SelectedNode { get; set; }
        private string Name { get; set; }

        #endregion

        #region private objects

        private List<MyTreeNode> selectedTypes = null;
        private EnvDTE.DTE envDTE;
        private ProjectItemsEvents projectItemsEvents;

        #endregion

        #region public methods

        // This method is called before opening any item that   
        // has the OpenInEditor attribute.  
        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        public void ProjectFinishedGenerating(Project project)
        {
            if (selectedTypes != null)
            {
                string validationErrors = string.Empty;
                string itemPath = null;
                string fileExtension = null;

                EnvDTE80.Events2 objEvents2;
                EnvDTE80.Solution2 solution = project.DTE.Solution as EnvDTE80.Solution2;

                OutputWindowManager outPutWindowmng = new OutputWindowManager();

                // Subscribe all the IDE events
                envDTE = project.DTE;
                objEvents2 = ((EnvDTE80.Events2)(envDTE.Events));

                // Subscribe the item added event
                projectItemsEvents = objEvents2.ProjectItemsEvents;
                projectItemsEvents.ItemAdded += ProjectItemsEvents_ItemAdded;

                // Write the post-build command to register the assembly.
                if (VSOptionsValidator.CanCreatePostBuildEvent(ref validationErrors))
                {
                    // Build the command to call.
                    string erpPath = Path.Combine(GeneralOptions.Instance.Path, "RegisterExtension.exe");

                    if (File.Exists(erpPath))
                    {
                        // Add command-line parameters.
                        string command = $"Call {string.Format("\"{0}\"", erpPath)} " +
                            $" {GeneralOptions.Instance.Company} {GeneralOptions.Instance.UserName}" +
                            $" {GeneralOptions.Instance.Password} {GeneralOptions.Instance.ProductLine}" +
                            $" $(TargetPath) {GeneralOptions.Instance.CommonExtension}";

                        EnvDTE.Properties configmg = project.Properties;
                        configmg.Item("PostBuildEvent").Value = command;

                        outPutWindowmng.WriteMessage("The post-build command was added to the project.", OutputWindowMessagesType.Message);

                        // Set start external program
                        string erpEpp = string.Format("Erp100L{0}.exe", GeneralOptions.Instance.ProductLine == 0 ? "E" : "P");

                        Configuration activeConfig = project.ConfigurationManager.ActiveConfiguration;
                        activeConfig.Properties.Item("StartAction").Value = prjStartAction.prjStartActionProgram;
                        activeConfig.Properties.Item("StartProgram").Value = Path.Combine(GeneralOptions.Instance.Path, erpEpp);

                        outPutWindowmng.WriteMessage("The start external program was set.", OutputWindowMessagesType.Message);
                    }
                    else
                    {
                        StringBuilder msg = new StringBuilder();

                        msg.Append("The post-build command could not be registered. \n");
                        msg.Append("The command line utility does not exist on the folder.");
                        msg.Append(GeneralOptions.Instance.Path);
                        msg.Append("Download it from https://developers.primaverabss.com/v10/como-automatizar-registo-extensoes/ \n");
                        
                        outPutWindowmng.WriteMessage(msg.ToString(), OutputWindowMessagesType.Error);
                    }
                }
                else
                {
                    outPutWindowmng.WriteMessage("The post-build event has not been configured because of the following validation issues:", OutputWindowMessagesType.Warning);
                    outPutWindowmng.WriteMessage(validationErrors, OutputWindowMessagesType.Message);
                    outPutWindowmng.WriteMessage("Check this on Tools | Options | PRIMAVERA Extensibility.", OutputWindowMessagesType.Message);
                }

                if (project.Kind == PrjKind.prjKindCSharpProject)
                {
                    itemPath = solution.GetProjectItemTemplate("PriClass.zip", "CSharp");
                    fileExtension = ".cs";
                }
                else
                {
                    itemPath = solution.GetProjectItemTemplate("PriClass.zip", "VisualBasic");
                    fileExtension = ".vb";
                }

                if (!String.IsNullOrEmpty(itemPath))
                {
                    // Add generic references
                    WizardHelper.AddBaseReferences(project,"Bas", GeneralOptions.Instance.Path);
                    WizardHelper.AddBaseReferences(project, "Ext", GeneralOptions.Instance.Path);

                    foreach (MyTreeNode type in selectedTypes)
                    {
                        this.SelectedNode = type;

                        ProjectItem rootFolder = project.ProjectItems.Cast<ProjectItem>()
                                    .FirstOrDefault(i => i.Name == type.Module) ?? project.ProjectItems.AddFolder(type.Module);

                        // Add the module reference.
                        WizardHelper.AddModuleReference(project, "Primavera.Extensibility." + type.Module);

                        // Add dependencies to the select modules.
                        WizardHelper.AddDependenciesReference(project, type.Module);

                        switch (type.ModuleType)
                        {
                            case "Editors":
                                rootFolder.ProjectItems.AddFromTemplate(itemPath, "Ui" + type.ClassName + fileExtension);
                                break;

                            case "Services":
                                rootFolder.ProjectItems.AddFromTemplate(itemPath, "Api" + type.ClassName + fileExtension);
                                break;
                        }
                    }
                }

                outPutWindowmng.WriteMessage("The project was created with success.");

                // UnSubscribing event.
                projectItemsEvents.ItemAdded -= ProjectItemsEvents_ItemAdded;
            }
        }

        // This method is only called for item templates,  
        // not for project templates.  
        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
            foreach (MyTreeNode type in selectedTypes)
            {
                this.SelectedNode = type;

                if (this.Name == "PriCustomTab.cs" || this.Name == "PriCustomTab.vb")
                {
                    WizardHelper.AddModuleReference(projectItem.ContainingProject, "Primavera.Extensibility.CustomTab");
                }

                // Add the module reference..,
                WizardHelper.AddModuleReference(projectItem.ContainingProject, "Primavera.Extensibility." + type.Module);

                ProjectItemsEvents_ItemAdded(projectItem);

                // Add dependencies to the select modules.
                WizardHelper.AddDependenciesReference(projectItem.ContainingProject, type.Module);
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
            using (ExtensibilityUI modulesForm = new ExtensibilityUI())
            {
                if (modulesForm.ShowDialog() == DialogResult.OK)
                {
                    selectedTypes = modulesForm.SelectedTypes.ToList();
                }
                else
                {
                    throw new WizardCancelledException();
                }
            }
            
        }

        // This method is only called for item templates,  
        // not for project templates.   
        public bool ShouldAddProjectItem(string filePath)
        {
            this.Name = filePath;
            return true;
        }
        #endregion

        #region private events
        /// <summary>
        /// Fire event when item is add to project.
        /// </summary>
        /// <param name = "ProjectItem" > The class added.</param>
        private void ProjectItemsEvents_ItemAdded(ProjectItem ProjectItem)
        {
            string itemPath = ProjectItem.Properties.Item("FullPath").Value.ToString();

            if (!string.IsNullOrEmpty(itemPath) && (File.GetAttributes(itemPath) & FileAttributes.Directory) != FileAttributes.Directory)
            {
                string readFile = File.ReadAllText(itemPath);

                readFile = readFile.Replace("$PriInheritsFrom$", this.SelectedNode.ClassName);
                readFile = readFile.Replace("$PriModule$", this.SelectedNode.Module);
                readFile = readFile.Replace("$ModuleType$", this.SelectedNode.ModuleType);

                File.WriteAllText(itemPath, readFile);
            }
        }
        #endregion
    }
}
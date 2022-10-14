using Primavera.Extensibility.Options;
using Primavera.Extensibility.Wizard;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Primavera.Extensibility.Presentation
{
    public partial class ExtensibilityUI : Form
    {
        #region private variables

        private List<MyTreeNode> childNodes = new List<MyTreeNode>();
        private List<MyTreeNode> rootNodes = new List<MyTreeNode>();

        #endregion

        #region internal properties
        internal List<MyTreeNode> SelectedTypes
        {
            get
            {
                return childNodes.Where(n => n!= null && n.Checked).ToList();
            }
        }
        #endregion

        #region  public methods
        public ExtensibilityUI()
        {
            InitializeComponent();

            LoadAssembly();

            LoadTreeView(childNodes);
        }

        private async void LoadAssembly()
        {
            var options = await GeneralOptions.GetLiveInstanceAsync();

            string InstallFolder = options.Path.ToString();

            if (!Directory.Exists(InstallFolder))
            {
                OutputWindowManager outPutWindowmng = new OutputWindowManager();
                StringBuilder msg =  new StringBuilder();
                
                msg.Append("Could not find PRIMAVERA installation folder {0}.\n");
                msg.Append("Check configuration on visual studio options.");

                outPutWindowmng.WriteMessage(msg.ToString(), OutputWindowMessagesType.Error);
                throw new DirectoryNotFoundException(String.Format(msg.ToString(), InstallFolder));
            }

            IEnumerable<string> folder = Directory.EnumerateFiles(InstallFolder, "*.dll");

            foreach (string file in folder)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);

                if (fileName.ToLower().StartsWith("primavera.extensibility") &&
                    !fileName.ToLower().EndsWith("attributes") &&
                    !fileName.ToLower().EndsWith("businessentities") &&
                    !fileName.ToLower().EndsWith("constants") &&
                    !fileName.ToLower().EndsWith("customcode") &&
                    !fileName.ToLower().EndsWith("customform") &&
                    !fileName.ToLower().EndsWith("customtab") &&
                    !fileName.ToLower().EndsWith("engine") &&
                    !fileName.ToLower().EndsWith("extensions") &&
                    !fileName.ToLower().EndsWith("integration") &&
                    !fileName.ToLower().EndsWith("patterns"))
                {
                    string moduleRootnode = fileName.Remove(0, 24);

                    // Build the root nodes collection
                    rootNodes.Add(new MyTreeNode() { Text = moduleRootnode });

                    // MEF load assemblies
                    Assembly assembly = Assembly.LoadFrom(file);

                    // Build the child nodes collection
                    foreach (var exportedType in assembly.GetExportedTypes())
                    {
                        MyTreeNode node = null;

                        string[] namespaceParts = exportedType.Namespace.Split('.');

                        if (namespaceParts.Length == 4)
                        {
                            node = new MyTreeNode()
                            {
                                Module = namespaceParts[2],
                                ModuleType = namespaceParts[3],
                                ClassName = exportedType.Name,
                                Text = exportedType.Name,
                                ParentNode = moduleRootnode,
                                Namespace = exportedType.Namespace
                            };
                        }

                        childNodes.Add(node);
                    }
                }
            }
        }

        /// <summary>
        /// Build the modules tree using the active nodes collection.
        /// </summary>
        /// <param name="myTreeNodes">Active node colecction.</param>
        private void LoadTreeView(List<MyTreeNode> myTreeNodes)
        {
            foreach (MyTreeNode node in rootNodes)
            {
                TreeNode parent = trw.Nodes.Add(node.Text);

                MyTreeNode childNodeEditors = new MyTreeNode() { Text = "Editors" };
                parent.Nodes.Add(childNodeEditors);

                MyTreeNode childNodeServices = new MyTreeNode() { Text = "Services" };
                parent.Nodes.Add(childNodeServices);

                foreach( MyTreeNode childNode in myTreeNodes)
                {
                    if (childNode != null && childNode.Module.ToLower() == node.Text.ToLower())
                    {
                        if (node.Text == "ElectronicDataInterchange" || node.Text == "Platform")
                        {
                            parent.Nodes.Remove(childNodeEditors);
                        }

                        if (childNode.Namespace.ToLower().EndsWith("editors"))
                        {
                            childNodeEditors.Nodes.Add(childNode);
                        }
                        else if (childNode.Namespace.ToLower().EndsWith("services"))
                        {
                            childNodeServices.Nodes.Add(childNode);
                        }
                    }
                }

                // Remove modules without child nodes in services or editors.
                if (childNodeEditors.Nodes.Count ==0 && childNodeServices.Nodes.Count == 0)
                {
                    parent.Nodes.Remove(parent);
                }

            }
        }
        #endregion

        #region private methods

        // Updates all child tree nodes recursively.
        private void CheckAllChildNodes(TreeNode treeNode, bool nodeChecked)
        {
            foreach (TreeNode node in treeNode.Nodes)
            {
                node.Checked = nodeChecked;
                if (node.Nodes.Count > 0)
                {
                    // If the current node has child nodes, call the CheckAllChildsNodes method recursively.
                    this.CheckAllChildNodes(node, nodeChecked);
                }
            }
        }

        private void FilterNodes()
        {
            this.trw.BeginUpdate();
            this.trw.Nodes.Clear();

            if (txtfilter.Text != string.Empty)
            {
                List<MyTreeNode> filterTreeNodes = new List<MyTreeNode>();

                foreach (MyTreeNode childNode in childNodes)
                {
                    if (childNode != null && childNode.Text.ToLower().Contains(this.txtfilter.Text.ToLower()))
                    {
                        filterTreeNodes.Add(childNode);
                    }
                }
                LoadTreeView(filterTreeNodes);
            }
            else
            {
                LoadTreeView(childNodes);
            }

            this.trw.EndUpdate();
        }

        #endregion

        #region events

        private void txtfilter_Leave(object sender, EventArgs e)
        {
            this.FilterNodes();
        }

        private void txtfilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Convert.ToInt32(e.KeyChar) == 13)
            {
                this.FilterNodes();
            }
        }

        private void trw_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // The code only executes if the user caused the checked state to change.
            if (e.Action != TreeViewAction.Unknown)
            {
                if (e.Node.Nodes.Count > 0)
                {
                    /* Calls the CheckAllChildNodes method, passing in the current 
                    Checked value of the TreeNode whose checked state changed. */
                    this.CheckAllChildNodes(e.Node, e.Node.Checked);
                }
            }
        }

        #endregion
    }
}

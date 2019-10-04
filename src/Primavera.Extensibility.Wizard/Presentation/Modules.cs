using Primavera.Extensibility.Options;
using Primavera.Extensibility.Wizard;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Primavera.Extensibility
{
    public partial class Modules : Form
    {
        #region private variables
        private List<MyTreeNode> allNodes = new List<MyTreeNode>();
        private List<TreeNode> NodesThatMatch = new List<TreeNode>();
        #endregion

        #region internal properties
        internal List<MyTreeNode> SelectedTypes
        {
            get
            {
                return allNodes.Where(t => t.Checked).ToList();
            }
        }
        #endregion

        #region  public methods
        public Modules()
        {
            InitializeComponent();
            LoadAssembly();
        }

        public void LoadAssembly()
        {
            string InstallFolder = GeneralOptions.Instance.Path;;

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
                    string trimName = fileName.Remove(0, 24);

                    TreeNode parent = trw.Nodes.Add(trimName);

                    MyTreeNode childNodeEditors = new MyTreeNode()
                    {
                        Text = "Editors",
                        IsDirectory = true
                    };
                    parent.Nodes.Add(childNodeEditors);

                    MyTreeNode childNodeServices = new MyTreeNode()
                    {
                        Text = "Services",
                        IsDirectory = true
                    };
                    parent.Nodes.Add(childNodeServices);

                    Assembly assembly = Assembly.LoadFrom(file);

                    foreach (var exportedType in assembly.GetExportedTypes())
                    {
                        if (trimName == "ElectronicDataInterchange" || trimName == "Platform")
                        {
                            parent.Nodes.Remove(childNodeEditors);
                        }

                        if (exportedType.Namespace.ToLower().EndsWith("editors"))
                        {
                            MyTreeNode node = new MyTreeNode()
                            {
                                Text = exportedType.Name,
                                NodeName = exportedType.Name,
                                Namespace = exportedType.Namespace,
                                IsDirectory= false
                            };
                            childNodeEditors.Nodes.Add(node);
                            allNodes.Add(node);
                        }

                        if (exportedType.Namespace.ToLower().EndsWith("services"))
                        {
                            MyTreeNode node = new MyTreeNode()
                            {
                                Text = exportedType.Name,
                                NodeName = exportedType.Name,
                                Namespace = exportedType.Namespace,
                                IsDirectory = false
                            };
                            childNodeServices.Nodes.Add(node);
                            allNodes.Add(node);
                        }
                    }
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
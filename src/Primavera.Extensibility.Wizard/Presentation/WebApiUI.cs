using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Primavera.Extensibility.Options;
using Primavera.Extensibility.Wizard;

namespace Primavera.Extensibility.Presentation
{
    public partial class WebApiUI : Form
    {
        #region internal properties
        internal List<TreeNode> SelectedTypes
        {
            get
            {
                return trw.Nodes.OfType<TreeNode>().Where(n => n != null && n.Checked).ToList();
            }
        }

        internal string ControllerName
        {
            get
            {
                return txtController.Text;
            }
        }
        #endregion

        #region private methods
        private void LoadAssembly()
        {
            string InstallFolder = GeneralOptions.Instance.Path; ;

            if (!Directory.Exists(InstallFolder))
            {
                OutputWindowManager outPutWindowmng = new OutputWindowManager();
                StringBuilder msg = new StringBuilder();

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
                    // Build the root nodes collection
                    TreeNode parent = trw.Nodes.Add(fileName.Remove(0, 24));
                }
            }
        }
        private void txtController_TextChanged(object sender, EventArgs e)
        {
            btn_ok.Enabled = !string.IsNullOrWhiteSpace(txtController.Text);
        }
        #endregion

        #region public
        public WebApiUI()
        {
            InitializeComponent();

            LoadAssembly();
        }
        #endregion
    }
}

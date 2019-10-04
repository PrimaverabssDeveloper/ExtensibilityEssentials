using System.Windows.Forms;

namespace Primavera.Extensibility
{
    internal class MyTreeNode : TreeNode
    {
        public string Namespace { get; set; }

        public string NodeName { get; set; }

        public bool IsDirectory { get; set; }
    }
}

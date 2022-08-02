using System.Windows.Forms;

namespace Primavera.Extensibility
{
    internal class MyTreeNode : TreeNode
    {
        public string Namespace { get; set; }

        public string Module { get; set; }

        public string ModuleType { get; set; }

        public string ClassName { get; set; }

        public string ParentNode { get; set; }
    }
}

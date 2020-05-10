using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;

namespace Primavera.Extensibility.Plugins
{
    internal class WepApiPlugin : IWizardPlugin
    {
        public void AddProjectItem(string filePath)
        {
            throw new NotImplementedException();
        }

        public void FinishedGenerating(Project project)
        {
            throw new NotImplementedException();
        }

        public void ItemFinishedGenerating(ProjectItem projectItem)
        {
            throw new NotImplementedException();
        }

        public void ItemsFinishedGenerating(ProjectItem projectItem)
        {
            throw new NotImplementedException();
        }
    }
}

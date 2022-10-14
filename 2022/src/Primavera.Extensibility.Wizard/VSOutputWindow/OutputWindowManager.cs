using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace Primavera.Extensibility.Wizard
{
    internal class OutputWindowManager: IDisposable
    {
        private readonly Guid customGuid = new Guid("0F44E2D1-F5FA-4d2d-AB30-22BE8ECD9789");
        private IVsOutputWindow outWindow;
        private IVsOutputWindowPane customPane;
        
        internal OutputWindowManager()
        {
            outWindow = Package.GetGlobalService(typeof(SVsOutputWindow)) as IVsOutputWindow;
            outWindow.GetPane(ref customGuid, out customPane);

            if (customPane == null)
            {
                string customTitle = "PRIMAVERA Extensibility Essentials";
                outWindow.CreatePane(ref customGuid, customTitle, 1, 1);
                outWindow.GetPane(ref customGuid, out customPane);
            }
        }

        public void WriteMessage(string message)
        {
            WriteMessage(message, OutputWindowMessagesType.Message);
        }

        public void WriteMessage(string message, OutputWindowMessagesType messagesType)
        {
            string msg = null;

            switch (messagesType)
            {
                case OutputWindowMessagesType.Message:
                    msg = $"{message} \n";
                    break;

                case OutputWindowMessagesType.Error:
                    msg = $"Error: - {message} \n";
                    break;

                case OutputWindowMessagesType.Warning:
                    msg = $"Warning: - {message} \n";
                    break;

            }

            customPane.OutputString(msg);
            customPane.Activate();
        }

        public void Dispose()
        {
            outWindow.DeletePane(customGuid);
        }
    }
}

using System;
using System.Runtime.InteropServices;
using System.Threading;
using Community.VisualStudio.Toolkit;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace Primavera.Extensibility.Options
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [Guid("750c3e8c-fd71-4123-a76c-65e08488858b")]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideOptionPage(typeof(DialogPageProvider.General),"PRIMAVERA Extensibility", "General", 0, 0, true)]
    [ProvideOptionPage(typeof(DialogPageProvider.WebApi), "PRIMAVERA Extensibility", "WebApi", 1, 1, true)]
    public sealed class VSOptionsPackage : ToolkitPackage
    {
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            GeneralOptions.Saved += GeneralOptions_Saved; ;
        }

        private void GeneralOptions_Saved(GeneralOptions obj)
        {
            throw new NotImplementedException();
        }
    }
}

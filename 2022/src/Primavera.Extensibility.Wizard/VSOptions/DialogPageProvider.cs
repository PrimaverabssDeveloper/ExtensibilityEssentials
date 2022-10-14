using System.Runtime.InteropServices;
using System.ComponentModel;
using Community.VisualStudio.Toolkit;

namespace Primavera.Extensibility.Options
{
    /// <summary>
    /// A provider for custom <see cref="DialogPage" /> implementations.
    /// </summary>
    internal class DialogPageProvider
    {
        [ComVisible(true)]
        public class General : BaseOptionPage<GeneralOptions> { }

        [ComVisible(true)]
        public class WebApi : BaseOptionPage<WebApiOptions> { }
    }
}

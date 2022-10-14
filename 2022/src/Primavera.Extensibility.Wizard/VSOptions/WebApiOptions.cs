using System.ComponentModel;
using Community.VisualStudio.Toolkit;

namespace Primavera.Extensibility.Options
{
    internal class WebApiOptions : BaseOptionModel<WebApiOptions>
    {
        [Category("WebAPI Settings")]
        [DisplayName("Installation Path")]
        [Description("PRIMAVERA WebAPI installation path.")]
        public string Path { get; set; } = @"C:\Program Files\PRIMAVERA\SG100\APL\WebApi\bin";
    }
}

namespace Primavera.Extensibility.Options
{
    /// <summary>
    /// A provider for custom <see cref="DialogPage" /> implementations.
    /// </summary>
    internal class DialogPageProvider
    {
        public class General : OptionPage<GeneralOptions> { }

        public class WebApi : OptionPage<WebApiOptions> { }
    }
}

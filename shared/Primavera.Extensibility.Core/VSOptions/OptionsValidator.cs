using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Primavera.Extensibility.Options
{
    internal static class VSOptionsValidator
    {
        internal static bool CanCreatePostBuildEvent(ref string validationErrors)
        {
            try
            {
                validationErrors = string.Empty;

                if (string.IsNullOrWhiteSpace(GeneralOptions.Instance.Company))
                    validationErrors += "  Company is not defined in Extension configuration. \n";

                if (string.IsNullOrWhiteSpace(GeneralOptions.Instance.Path))
                    validationErrors += "  Installation path is not defined in Extension configuration. \n";

                if (string.IsNullOrWhiteSpace(GeneralOptions.Instance.UserName))
                    validationErrors += "  User name is not defined in Extension configuration. \n";

                if (string.IsNullOrWhiteSpace(GeneralOptions.Instance.ProductLine.ToString()))
                    validationErrors += "  Product line is not defined in Extension configuration. \n";
                else
                    if (!((GeneralOptions.Instance.ProductLine >= 0) && (GeneralOptions.Instance.ProductLine <= 1)))
                    validationErrors += "  Product line is not correctly defined in Extension configuration. Value must be 0 or 1. \n";

                if (string.IsNullOrWhiteSpace(GeneralOptions.Instance.CommonExtension.ToString()))
                    validationErrors += "  Common extension is not defined in Extension configuration. \n";
            }
            catch (Exception ex)
            {
                validationErrors = ex.Message;
            }

            return (string.IsNullOrWhiteSpace(validationErrors));
        }
    }
}

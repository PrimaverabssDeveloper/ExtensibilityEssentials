using System;
using System.Collections.Generic;
using System.IO;
using Primavera.Extensibility.Options;

namespace Primavera.Extensibility
{
    internal static class DependenciesDictionary
    {
        #region Dependencies Dictionary

        public static Dictionary<string, string> Dependencies
        {
            get
            {
                return new Dictionary<string, string>()
                {
                    { "Sales", "VndBEXXX;IVndBSXXX" },
                    { "Purchases", "CmpBEXXX;ICmpBSXXX" },
                    { "Internal", "IntBEXXX;IintBSXXX" },
                    { "Base", "BasBEXXX;IBasBSXXX" },
                    { "Inventory", "InvBEXXX;IInvBSXXX" },
                    { "Accounting", "CblBEXXX;ICblBSXXX" },
                    { "ContactsOpportunities", "CrmBEXXX;ICrmBSXXX" },
                    { "CashManagement", "TesBEXXX;ITesBSXXX" },
                    { "ContractManagement", "PcmBEXXX;IPcmBSXXX" },
                    { "EquipmentsFixedAssets", "EapBEXXX;IEapBSXXX" },
                    { "PayablesReceivables", "CctBEXXX;ICctBSXXX" },
                    { "HumanResources", "RhpBEXXX;IRhpBSXXX" },
                    { "Production","GprBEXXX;IGprBSXXX" }
                };
            }
        }

        public static Dictionary<string, string> BaseDependencies
        {
            get
            {
                return new Dictionary<string, string>()
                {
                    { "BasStdPlatBS", "StdPlatBSXXX" },
                    { "BasStdBE", "StdBEXXX" },
                    { "BaseErpBS", "ErpBSXXX" },
                    { "ExtBusinessEntities", "Primavera.Extensibility.BusinessEntities" },
                    { "ExtIntegration", "Primavera.Extensibility.Integration" },
                    { "ExtAttributes", "Primavera.Extensibility.Attributes" },
                    { "ApiDocumentExtendedEntities", "Primavera.WebAPI.Integration" },
                    { "PlatformRuntime","Primavera.Platform.Runtime" }
                };
            }
        }

        public static Dictionary<string, string> ExtensibilityBaseDependencies
        {
            get
            {
                return new Dictionary<string, string>()
                {
                    { "BusinessEntities", "Primavera.Extensibility.BusinessEntities" },
                    { "Integration", "Primavera.Extensibility.Integration" },
                    { "Attributes", "Primavera.Extensibility.Attributes" }
                };
            }
        }

        public static Dictionary<string, string> WebApiBaseDependencies
        {
            get
            {
                return new Dictionary<string, string>()
                {
                    { "DocumentExtendedEntities", "Primavera.WebAPI.Integration" }
                };
            }
        }

        #endregion
    }
}
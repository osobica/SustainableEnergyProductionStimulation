﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Acme.Seps.Domain.Parameter.Infrastructure
{
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Parameter
    {

        private static global::System.Resources.ResourceManager resourceMan;

        private static global::System.Globalization.CultureInfo resourceCulture;

        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Parameter()
        {
        }

        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Acme.Seps.Domain.Parameter.Infrastructure.Parameter", typeof(Parameter).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }

        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture
        {
            get
            {
                return resourceCulture;
            }
            set
            {
                resourceCulture = value;
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Lower production limit must be zero or above..
        /// </summary>
        public static string BelowZeroLowerProductionLimitException
        {
            get
            {
                return ResourceManager.GetString("BelowZeroLowerProductionLimitException", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Lower rate must be zero or above..
        /// </summary>
        public static string BelowZeroLowerRateException
        {
            get
            {
                return ResourceManager.GetString("BelowZeroLowerRateException", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Upper rate must be zero or above..
        /// </summary>
        public static string BelowZeroUpperRateException
        {
            get
            {
                return ResourceManager.GetString("BelowZeroUpperRateException", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Cogeneration Parameter Service must be set..
        /// </summary>
        public static string CogenerationParameterServiceException
        {
            get
            {
                return ResourceManager.GetString("CogenerationParameterServiceException", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Consumer price index must be set..
        /// </summary>
        public static string ConsumerPriceIndexNotSetException
        {
            get
            {
                return ResourceManager.GetString("ConsumerPriceIndexNotSetException", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Created {0} for period {1} with amount {2:C}..
        /// </summary>
        public static string InsertParameterLog
        {
            get
            {
                return ResourceManager.GetString("InsertParameterLog", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Created {0} for period {1} with amounts {2:C}/{2:C}..
        /// </summary>
        public static string InsertTariffLog
        {
            get
            {
                return ResourceManager.GetString("InsertTariffLog", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Upper rate must be higher than lower rate..
        /// </summary>
        public static string LowerRateAboveUpperException
        {
            get
            {
                return ResourceManager.GetString("LowerRateAboveUpperException", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Year and month must be set properly..
        /// </summary>
        public static string MonthlyParameterException
        {
            get
            {
                return ResourceManager.GetString("MonthlyParameterException", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Natural gas selling price must be set..
        /// </summary>
        public static string NaturalGasSellingPriceNotSetException
        {
            get
            {
                return ResourceManager.GetString("NaturalGasSellingPriceNotSetException", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Amount must be higher than zero..
        /// </summary>
        public static string ParameterAmountBelowOrZeroException
        {
            get
            {
                return ResourceManager.GetString("ParameterAmountBelowOrZeroException", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Decimal places must be a positive number..
        /// </summary>
        public static string ParameterDecimalPlacesBelowZeroException
        {
            get
            {
                return ResourceManager.GetString("ParameterDecimalPlacesBelowZeroException", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Remark must be set..
        /// </summary>
        public static string RemarkNotSetException
        {
            get
            {
                return ResourceManager.GetString("RemarkNotSetException", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Renewable energy source tariff period must be in sequence..
        /// </summary>
        public static string RenewableEnergySourceTariffPeriodException
        {
            get
            {
                return ResourceManager.GetString("RenewableEnergySourceTariffPeriodException", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Updated {0} for period {1} with amount {2:C}..
        /// </summary>
        public static string UpdateParameterLog
        {
            get
            {
                return ResourceManager.GetString("UpdateParameterLog", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Updated {0} for period {1} with amounts {2:C}/{2:C}..
        /// </summary>
        public static string UpdateTariffLog
        {
            get
            {
                return ResourceManager.GetString("UpdateTariffLog", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Year must be set properly..
        /// </summary>
        public static string YearlyParameterException
        {
            get
            {
                return ResourceManager.GetString("YearlyParameterException", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Years&apos; Natural Gas Selling Prices must be set..
        /// </summary>
        public static string YearsNaturalGasSellingPricesException
        {
            get
            {
                return ResourceManager.GetString("YearsNaturalGasSellingPricesException", resourceCulture);
            }
        }
    }
}

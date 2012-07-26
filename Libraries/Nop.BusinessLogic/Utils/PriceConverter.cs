
namespace NopSolutions.NopCommerce.BusinessLogic.Utils
{
    using Configuration.Settings;

    ///<summary>
    ///</summary>
    public class PriceConverter
    {
        ///<summary>
        ///</summary>
        ///<param name="amount"></param>
        ///<returns></returns>
        public static decimal ToUsd(decimal amount)
        {
            decimal usdToByr = SettingManager.GetSettingValueDecimalNative("PaymentMethods.UsdRate");
            return amount / usdToByr;
        }

        public static long ToBYR(decimal amount)
        {
            decimal byrToUsd = SettingManager.GetSettingValueDecimalNative("PaymentMethods.UsdRate");
            return (long)(amount * byrToUsd);
        }
    }
}

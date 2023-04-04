using Prestadito.Security.Infrastructure.Proxies.Settings.Models.Parameters;

namespace Prestadito.Security.Application.Manager.Utilities
{
    public static class SettingProxyUtility
    {
        public static void ParameterToValueInt(this ParameterModel parameter, ref int value)
        {
            if (int.TryParse(parameter.StrValue, out int tempValue))
            {
                value = tempValue;
            }
        }
    }
}

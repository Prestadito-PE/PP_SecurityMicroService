using Prestadito.Security.Infrastructure.Proxies.Settings.Models;
using Prestadito.Security.Infrastructure.Proxies.Settings.Models.Parameters;

namespace Prestadito.Security.Application.Manager.Utilities
{
    public static class SettingProxyUtility
    {
        public static void ParameterToValueInt(this ResponseProxyModel<ParameterModel>? parameter, ref int value)
        {
            if (parameter is not null && !parameter.Error)
            {
                if (int.TryParse(parameter.Item.StrValue, out int tempValue))
                {
                    value = tempValue;
                }
            }
        }
    }
}

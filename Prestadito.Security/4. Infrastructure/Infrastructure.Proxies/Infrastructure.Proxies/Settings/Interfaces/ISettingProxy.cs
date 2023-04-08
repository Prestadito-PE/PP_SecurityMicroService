using Prestadito.Security.Infrastructure.Proxies.Settings.Models;
using Prestadito.Security.Infrastructure.Proxies.Settings.Models.Parameters;

namespace Prestadito.Security.Infrastructure.Proxies.Settings.Interfaces
{
    public interface ISettingProxy
    {
        ValueTask<ResponseProxyModel<ParameterModel>?> GetParameterByCode(string parameterCode);
    }
}

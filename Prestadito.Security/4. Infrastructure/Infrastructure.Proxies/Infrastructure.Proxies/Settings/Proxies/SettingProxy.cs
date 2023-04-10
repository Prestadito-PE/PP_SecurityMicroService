using Microsoft.Extensions.Configuration;
using Prestadito.Security.Infrastructure.Proxies.Settings.Endpoints;
using Prestadito.Security.Infrastructure.Proxies.Settings.Interfaces;
using Prestadito.Security.Infrastructure.Proxies.Settings.Models;
using Prestadito.Security.Infrastructure.Proxies.Settings.Models.Parameters;
using System.Net.Http.Json;

namespace Prestadito.Security.Infrastructure.Proxies.Settings.Proxies
{
    public class SettingProxy : ISettingProxy
    {
        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;

        public SettingProxy(HttpClient _httpClient, IConfiguration _configuration)
        {
            httpClient = _httpClient;
            configuration = _configuration;
        }

        public async ValueTask<ResponseProxyModel<ParameterModel>?> GetParameterByCode(string parameterCode)
        {
            try
            {
                var path = BuildPath($"{SettingEndpoints.GetParametersByCodePath}{parameterCode}");
                var response = await httpClient.GetAsync(path);

                var responseModel = await response.Content.ReadFromJsonAsync<ResponseProxyModel<ParameterModel>>();
                return responseModel;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string BuildPath(string endpoint)
        {
            return $"{configuration[SettingEndpoints.SettingServiceUrl]}{endpoint}";
        }
    }
}

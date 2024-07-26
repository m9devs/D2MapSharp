using System.IO;

using D2MapApi.Core.Wrapper;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace D2MapApi.Core.Extensions
{
    public static class CoreExtensions
    {
        public static void RegisterCoreServices(this IServiceCollection p_services, IConfiguration p_config)
        {
            p_services.AddSingleton<IMapService, MapService>();
            if(!Directory.Exists(p_config["Diablo2Directory"]))
            {
                throw new System.Exception($"Provided invalid diablo 2 directory: {p_config["Diablo2Directory"]}");
            }
            MapDll.Initialize(p_config["Diablo2Directory"]);
        }
    }
}

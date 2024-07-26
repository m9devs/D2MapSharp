using D2MapApi.Common.DataStructures;
using D2MapApi.Common.Enumerations.GameData;
using D2MapApi.Core;
using D2MapApi.Core.Models;

using Microsoft.Extensions.Logging;

namespace D2MapApi.Server.Grpc.Services.MapData;

internal class MapDataService(ILogger<MapDataService> i_logger,
                              IMapService             i_mapService) : IMapDataService
{
    public async Task<D2AreaMap> GetD2AreaMapAsync(uint p_seed, D2Difficulty p_d2Difficulty, D2Area p_d2Area)
    {
        return await i_mapService.GetCollisionMapAsync(p_seed, p_d2Difficulty, p_d2Area);
    }
}
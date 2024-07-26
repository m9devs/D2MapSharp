using D2MapApi.Common.Enumerations.GameData;
using D2MapApi.Server.Grpc.Protos.Api.V1.Map;
using D2MapApi.Server.Grpc.Services.MapData;
using D2MapApi.Server.Grpc.Services.ObjectTranslation;

using Grpc.Core;

using Microsoft.Extensions.Logging;

namespace D2MapApi.Server.Grpc.Services.GrpcServiceImplementations;

internal class MapServiceImplementation(ILogger<MapServiceImplementation> i_logger,
                                        IMapDataService                   i_mapDataService,
                                        MapObjectTranslationService       i_mapObjectTranslationService) : Map.MapBase
{
    public override async Task<G_MapDataResponse> GetMapData(G_MapDataRequest p_request, ServerCallContext p_context)
    {
        i_logger.LogInformation("Received request for map data");
        var d2AreaMap = await i_mapDataService.GetD2AreaMapAsync(p_request.Seed, (D2Difficulty)p_request.Difficulty, (D2Area)p_request.Area);

        return new G_MapDataResponse
               {
                   Success = new G_MapDataSuccessResponse
                             {
                                 D2AreaMap = i_mapObjectTranslationService.ToD2AreaMapMessage(d2AreaMap)
                             }
               };
    }
}
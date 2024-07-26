using D2MapApi.Common.DataStructures;
using D2MapApi.Common.Enumerations.GameData;
using D2MapApi.Core.Models;

namespace D2MapApi.Server.Grpc.Services.MapData;

public interface IMapDataService
{
    public Task<D2AreaMap> GetD2AreaMapAsync(uint p_seed, D2Difficulty p_d2Difficulty, D2Area p_d2Area);
}
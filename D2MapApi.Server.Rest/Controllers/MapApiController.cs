using System.Threading.Tasks;

using D2MapApi.Core;
using D2MapApi.Server.Rest.Controllers.Models;

using Microsoft.AspNetCore.Mvc;

namespace D2MapApi.Server.Rest.Controllers
{
    [ApiController]
    [Route("/maps")]
    public class MapApiController : ControllerBase
    {
        private readonly IMapService m_mapService;

        public MapApiController(IMapService p_mapService)
        {
            m_mapService = p_mapService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] MapParameters p_parameters)
        {
           return Ok(await m_mapService.GetCollisionMapAsync(p_parameters.MapId, p_parameters.Difficulty.GetValueOrDefault(), p_parameters.Area.GetValueOrDefault()));
        }
    }
}

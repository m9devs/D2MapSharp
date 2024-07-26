using System.ComponentModel.DataAnnotations;

using D2MapApi.Common.Enumerations.GameData;

using Microsoft.AspNetCore.Mvc;

namespace D2MapApi.Server.Rest.Controllers.Models
{
    public class MapParameters
    {
        [Required]
        [FromQuery]
        public uint MapId { get; set; }

        [Required]
        [FromQuery]
        public D2Difficulty? Difficulty { get; set; }

        [Required]
        [FromQuery]
        public D2Area? Area { get; set; }
    }
}

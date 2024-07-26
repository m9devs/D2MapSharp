using System.Numerics;

using CommandLine;

using D2MapApi.Client.CLI.Grpc.DataStructures.ProgramArguments;
using D2MapApi.Client.CLI.Grpc.Global.IO.Directories;
using D2MapApi.Common.Enumerations.Extensions;
using D2MapApi.Common.Enumerations.GameData;
using D2MapApi.Common.Lookup;
using D2MapApi.Server.Grpc.Protos.Api.V1.Connectivity;
using D2MapApi.Server.Grpc.Protos.Api.V1.Map;

using Grpc.Net.Client;

using Microsoft.Extensions.Logging;

using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace D2MapApi.Client.CLI.Grpc.Services;

internal class MapperApplicationService(ILogger<MapperApplicationService> i_logger)
{
    internal async Task RunAsync(ParserResult<object> p_parseResult)
    {
        i_logger.LogInformation("Running");
        await p_parseResult.WithParsed<MapOptions>(RunMapOptions)
                           .WithParsedAsync<ActOptions>(RunActOptions);
    }

    private Task RunActOptions(ActOptions p_arg)
    {
        throw new NotImplementedException();
    }

    private void RunMapOptions(MapOptions p_mapOptions)
    {
        var channel = GrpcChannel.ForAddress("http://localhost:5000");

        var connectionClient = new Connectivity.ConnectivityClient(channel);
        connectionClient.CheckServerConnection(new G_ConnectionCheckRequest());

        var mapClient = new Map.MapClient(channel);
        var mapData = mapClient.GetMapData(new G_MapDataRequest
                                           {
                                               Difficulty = (G_MapDataRequest.Types.G_Difficulty)p_mapOptions.D2Difficulty,
                                               Area       = p_mapOptions.MapId,
                                               Seed       = p_mapOptions.Seed
                                           });

        switch ( mapData.MapResponseCase )
        {
            case G_MapDataResponse.MapResponseOneofCase.None:
                break;
            case G_MapDataResponse.MapResponseOneofCase.Success:
                const int scale = 3;

                using ( var image = new Image<Rgba32>(mapData.Success.D2AreaMap.Width * scale, mapData.Success.D2AreaMap.Height * scale) )
                {
                    image.Mutate(p_ctx => p_ctx.Fill(Color.Black));

                    for ( var i = 0; i < image.Height; i += scale )
                    {
                        for ( var j = 0; j < image.Width; j += scale )
                        {
                            var currentBlock = mapData.Success.D2AreaMap.CollisionData.CollisionRows[i / scale].Blocks[j / scale];
                            var color        = MapBlockColorLookup.ColorLookup.GetValueOrDefault(currentBlock);

                            for ( var k = 0; k < scale; k++ )
                            {
                                for ( var l = 0; l < scale; l++ )
                                {
                                    image[j + k, i + l] = new Rgba32(color);
                                }
                            }
                        }
                    }

                    foreach ( var gameObject in mapData.Success.D2AreaMap.Objects )
                    {
                        var adjustedObjectPosition = new Vector2(gameObject.Position.XPosition * scale, gameObject.Position.YPosition * scale);
                        image.Mutate(p_ctx =>
                                     {
                                         p_ctx.FillPolygon(Color.Red, [
                                                                          new PointF(adjustedObjectPosition.X - gameObject.Width * scale, adjustedObjectPosition.Y + gameObject.Height * scale),
                                                                          new PointF(adjustedObjectPosition.X + gameObject.Width * scale, adjustedObjectPosition.Y + gameObject.Height * scale),
                                                                          new PointF(adjustedObjectPosition.X + gameObject.Width * scale, adjustedObjectPosition.Y - gameObject.Height * scale),
                                                                          new PointF(adjustedObjectPosition.X - gameObject.Width * scale, adjustedObjectPosition.Y - gameObject.Height * scale)
                                                                      ]);
                                     });
                    }

                    foreach ( var npc in mapData.Success.D2AreaMap.Npcs )
                    {
                        if ( npc.NpcId > 733 ) continue;

                        var adjustedNpcPosition = new Vector2(npc.NpcPosition.XPosition * scale, npc.NpcPosition.YPosition * scale);

                        image.Mutate(p_ctx =>
                                     {
                                         p_ctx.FillPolygon(Color.Blue, [
                                                                           new PointF(adjustedNpcPosition.X - 2 * scale, adjustedNpcPosition.Y + 2 * scale),
                                                                           new PointF(adjustedNpcPosition.X + 2 * scale, adjustedNpcPosition.Y + 2 * scale),
                                                                           new PointF(adjustedNpcPosition.X + 2 * scale, adjustedNpcPosition.Y - 2 * scale),
                                                                           new PointF(adjustedNpcPosition.X - 2 * scale, adjustedNpcPosition.Y - 2 * scale)
                                                                       ]);

                                         p_ctx.DrawText(( (D2Npc)npc.NpcId ).ToFriendlyString(), SystemFonts.CreateFont("Impact", 8 * scale, FontStyle.Bold), Brushes.Solid(Color.Black),
                                                        Pens.Solid(Color.White, .25f),
                                                        new PointF(adjustedNpcPosition.X, adjustedNpcPosition.Y));
                                     });
                    }

                    foreach ( var accessibleArea in mapData.Success.D2AreaMap.AccessibleAreas )
                    {
                        var adjustedAreaPosition = new Vector2(accessibleArea.ExitPosition.XPosition * scale, accessibleArea.ExitPosition.YPosition * scale);

                        image.Mutate(p_ctx =>
                                     {
                                         p_ctx.FillPolygon(Color.Magenta, [
                                                                              new PointF(adjustedAreaPosition.X - 4 * scale, adjustedAreaPosition.Y + 4 * scale),
                                                                              new PointF(adjustedAreaPosition.X + 4 * scale, adjustedAreaPosition.Y + 4 * scale),
                                                                              new PointF(adjustedAreaPosition.X + 4 * scale, adjustedAreaPosition.Y - 4 * scale),
                                                                              new PointF(adjustedAreaPosition.X - 4 * scale, adjustedAreaPosition.Y - 4 * scale)
                                                                          ]);

                                         p_ctx.DrawText(( (D2Area)accessibleArea.AreaId ).ToFriendlyString(), SystemFonts.CreateFont("Impact", 8 * scale, FontStyle.Bold), Brushes.Solid(Color.Black),
                                                        Pens.Solid(Color.White, .25f),
                                                        new PointF(adjustedAreaPosition.X, adjustedAreaPosition.Y));
                                     });
                    }

                    image.Mutate(p_ctx =>
                                 {
                                     p_ctx.Skew(0, -30);
                                     p_ctx.Rotate(62.5f);
                                 });

                    image.SaveAsPng(Path.Combine(ApplicationDirectories.LogsDataPath, $"MAP_{(D2Area)p_mapOptions.MapId}_{p_mapOptions.D2Difficulty.ToFriendlyString()}_{p_mapOptions.Seed}.png"),
                                    new PngEncoder
                                    {
                                        CompressionLevel = PngCompressionLevel.DefaultCompression
                                    });
                }

                break;
            case G_MapDataResponse.MapResponseOneofCase.Failure:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
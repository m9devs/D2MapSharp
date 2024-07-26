using D2MapApi.Server.Grpc.Protos.Api.V1.Connectivity;

using Grpc.Core;

using Microsoft.Extensions.Logging;

namespace D2MapApi.Server.Grpc.Services.GrpcServiceImplementations;

internal class ConnectivityServiceImplementation(ILogger<ConnectivityServiceImplementation> i_logger) : Connectivity.ConnectivityBase
{
   public override async Task<G_ConnectionCheckResponse> CheckServerConnection(G_ConnectionCheckRequest p_request, ServerCallContext p_context)
   {
      i_logger.LogInformation("Client at {Peer} checked for server availability", p_context.Peer);
      return await Task.FromResult(new G_ConnectionCheckResponse());
   }
}
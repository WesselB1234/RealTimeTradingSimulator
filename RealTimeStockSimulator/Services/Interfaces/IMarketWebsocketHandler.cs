using RealTimeStockSimulator.Models;

namespace RealTimeStockSimulator.Services.Interfaces
{
    public interface IMarketWebsocketHandler
    {
        Task HandleMarketWebSocketPayload(IncomingMarketWebsocketAsset incomingTradable, CancellationToken cancellationToken);
    }
}

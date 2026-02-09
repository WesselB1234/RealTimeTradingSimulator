using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Repositories.Interfaces;
using StackExchange.Redis;
using System.Text.Json;
using IServer = StackExchange.Redis.IServer;

namespace RealTimeStockSimulator.Repositories
{
    public class RedisAssetPriceInfosRepository : IAssetPriceInfosRepository
    {
        private IConfiguration _configuration;
        private IDatabase _redisTradablePriceInfosDb;
        private IServer _redisTradablePriceInfosServer;

        public RedisAssetPriceInfosRepository(IConfiguration configuration)
        {
            _configuration = configuration;

            InitRedisTradablePriceInfoDbConnection();
        }

        private void InitRedisTradablePriceInfoDbConnection()
        {
            ConnectionMultiplexer muxer = ConnectionMultiplexer.Connect(
                new ConfigurationOptions
                {
                    EndPoints = {
                        {
                            _configuration.GetValue<string>("RedisConnectionStrings:RedisTradablePriceInfosDb:EndPointUrl"),
                            _configuration.GetValue<int>("RedisConnectionStrings:RedisTradablePriceInfosDb:EndPointPort")
                        }
                    },
                    User = _configuration.GetValue<string>("RedisConnectionStrings:RedisTradablePriceInfosDb:User"),
                    Password = _configuration.GetValue<string>("RedisConnectionStrings:RedisTradablePriceInfosDb:Password")
                });

            _redisTradablePriceInfosDb = muxer.GetDatabase();
            _redisTradablePriceInfosServer = muxer.GetServer(muxer.GetEndPoints().First());
            _redisTradablePriceInfosDb.Execute("FLUSHDB");
        }

        public AssetPriceInfos? GetPriceInfosBySymbol(string symbol)
        {
            if (_redisTradablePriceInfosDb.KeyExists(symbol))
            {
                return JsonSerializer.Deserialize<AssetPriceInfos>(_redisTradablePriceInfosDb.StringGet(symbol).ToString());
            }

            return null;
        }

        public List<string> GetAllKeys()
        {
            return _redisTradablePriceInfosServer.Keys(pattern: "*")
                         .Select(k => (string)k)
                         .ToList();
        }

        public void SetPriceInfosBySymbol(string symbol, AssetPriceInfos priceInfos)
        {
            _redisTradablePriceInfosDb.StringSet(symbol, JsonSerializer.Serialize(priceInfos));
        }
    }
}

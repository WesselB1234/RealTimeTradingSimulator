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
        private IDatabase _redisAssetPriceInfosDb;
        private IServer _redisAssetPriceInfosServer;

        public RedisAssetPriceInfosRepository(IConfiguration configuration)
        {
            _configuration = configuration;

            InitRedisAssetPriceInfoDbConnection();
        }

        private void InitRedisAssetPriceInfoDbConnection()
        {
            ConnectionMultiplexer muxer = ConnectionMultiplexer.Connect(
                new ConfigurationOptions
                {
                    EndPoints = {
                        {
                            _configuration.GetValue<string>("RedisConnectionStrings:RedisAssetPriceInfosDb:EndPointUrl"),
                            _configuration.GetValue<int>("RedisConnectionStrings:RedisAssetPriceInfosDb:EndPointPort")
                        }
                    },
                    User = _configuration.GetValue<string>("RedisConnectionStrings:RedisAssetPriceInfosDb:User"),
                    Password = _configuration.GetValue<string>("RedisConnectionStrings:RedisAssetPriceInfosDb:Password")
                });

            _redisAssetPriceInfosDb = muxer.GetDatabase();
            _redisAssetPriceInfosServer = muxer.GetServer(muxer.GetEndPoints().First());
            _redisAssetPriceInfosDb.Execute("FLUSHDB");
        }

        public AssetPriceInfos? GetPriceInfosBySymbol(string symbol)
        {
            if (_redisAssetPriceInfosDb.KeyExists(symbol))
            {
                return JsonSerializer.Deserialize<AssetPriceInfos>(_redisAssetPriceInfosDb.StringGet(symbol).ToString());
            }

            return null;
        }

        public List<string> GetAllKeys()
        {
            return _redisAssetPriceInfosServer.Keys(pattern: "*").Select(k => (string)k).ToList();
        }

        public void SetPriceInfosBySymbol(string symbol, AssetPriceInfos priceInfos)
        {
            _redisAssetPriceInfosDb.StringSet(symbol, JsonSerializer.Serialize(priceInfos));
        }
    }
}

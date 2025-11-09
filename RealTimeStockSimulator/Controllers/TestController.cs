using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace RealTimeStockSimulator.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            Console.WriteLine("not found");

            var muxer = ConnectionMultiplexer.Connect(
            new ConfigurationOptions
                {
                    EndPoints = { { "redis-13367.c328.europe-west3-1.gce.redns.redis-cloud.com", 13367 } },
                    User = "default",
                    Password = "***"
            }
            );
            var db = muxer.GetDatabase();

            db.StringSet("foo", "bar");
            RedisValue result = db.StringGet("foo");
            Console.WriteLine(result); // >>> bar

            return NotFound();
        }
    }
}

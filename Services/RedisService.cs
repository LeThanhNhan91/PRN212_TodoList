using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class RedisService
    {
        private readonly IConnectionMultiplexer _redis;

        public RedisService(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public async Task PublishMessageAsync(string channel, string message)
        {
            var subscriber = _redis.GetSubscriber();
            await subscriber.PublishAsync(channel, message);
        }
    }
}

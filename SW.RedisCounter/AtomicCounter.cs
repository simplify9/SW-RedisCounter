using Microsoft.AspNetCore.Hosting;
using StackExchange.Redis;
using SW.PrimitiveTypes;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SW.RedisCounter
{
    public class AtomicCounter : IAtomicCounter
    {
        private readonly IConnectionMultiplexer redisConnection;
        private readonly string applicationName;
        private readonly string environmentName;

        public AtomicCounter(RedisOptions redisOptions, IWebHostEnvironment environment)
        {
            var options = new ConfigurationOptions
            {
                Password = redisOptions.Password,
                Ssl = true,
            };

            var servers = redisOptions.Server.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
            servers.ForEach(i => options.EndPoints.Add(i));

            redisConnection = ConnectionMultiplexer.Connect(options);

            applicationName = redisOptions.ApplicationName;
            environmentName = environment.EnvironmentName;
        }

        public Task<long> IncrementAsync(string counterName, long increment = 1)
        {
            var db = redisConnection.GetDatabase();
            return db.StringIncrementAsync(BuildFullyQualifiedKeyName(counterName), increment);
        }

        public Task ResetAsync(string counterName)
        {
            var db = redisConnection.GetDatabase();
            return db.KeyDeleteAsync(BuildFullyQualifiedKeyName(counterName));
        }

        private string BuildFullyQualifiedKeyName(string name)
        {
            return $"{applicationName}:{environmentName}:{name}".ToLower();
        }
    }
}

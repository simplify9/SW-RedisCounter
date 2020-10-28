using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SW.RedisCounter
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddRedisCounter(this IServiceCollection services, Action<RedisOptions> configure = null)
        {
            var redisOptions = new RedisOptions();
            if (configure != null)
                configure.Invoke(redisOptions);

            var serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetRequiredService<IConfiguration>().GetSection(RedisOptions.ConfigurationSection).Bind(redisOptions);

            services.AddSingleton(redisOptions);
            services.AddSingleton<IAtomicCounter, AtomicCounter>();

            return services;
        }
    }
}

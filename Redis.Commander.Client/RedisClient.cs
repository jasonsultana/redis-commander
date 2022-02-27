using System;
using System.Threading.Tasks;
using Redis.Commander.Client.Contracts;
using Redis.Commander.Model;
using StackExchange.Redis;

namespace Redis.Commander.Client
{
    public class RedisClient : IRedisClient
    {
        public async Task<string> GetAsync(Connection connection, string key)
        {
            // Normally we'd want to keep the connection open, but since the
            // user can switch between connections at any time and the app
            // will probably be short-lived anyway, close the connection after
            // the command has finished.
            using var redis = GetRedis(connection);

            var db = redis.GetDatabase();

            return await db.StringGetAsync(key);
        }

        public async Task DeleteAsync(Connection connection, string key)
        {
            using var redis = GetRedis(connection);

            var db = redis.GetDatabase();

            await db.KeyDeleteAsync(key);
        }

        private ConnectionMultiplexer GetRedis(Connection connection)
        {
            var redis = ConnectionMultiplexer.Connect(new ConfigurationOptions()
            {
                Password = connection.Pass,
                Ssl = connection.UseSSL,
                User = connection.User,
                EndPoints =
                {
                    { connection.HostUrl, connection.Port }
                }
            });

            return redis;
        }
    }
}


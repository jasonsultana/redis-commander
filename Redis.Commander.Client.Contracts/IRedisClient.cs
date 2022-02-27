using System;
using System.Threading.Tasks;
using Redis.Commander.Model;

namespace Redis.Commander.Client.Contracts
{
    public interface IRedisClient
    {
        Task<string> GetAsync(Connection connection, string key);

        Task DeleteAsync(Connection connection, string key);
    }
}


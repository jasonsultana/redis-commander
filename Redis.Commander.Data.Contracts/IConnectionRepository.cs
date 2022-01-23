using System;
using System.Threading.Tasks;
using Redis.Commander.Model;
using Redis.Commander.Model.DTOs;

namespace Redis.Commander.Data.Contracts
{
    public interface IConnectionRepository
    {
        Task AddAsync(Connection connection);

        Task UpdateAsync(Connection connection);

        Task<ListConnectionsDto[]> ListConnectionsAsync();

        Task<Connection> GetAsync(int connectionId);
    }
}

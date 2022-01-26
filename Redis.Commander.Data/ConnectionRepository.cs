using System;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using Redis.Commander.Data.Contracts;
using Redis.Commander.Model;
using Redis.Commander.Model.DTOs;
using Dapper;
using Dapper.Contrib;
using System.Linq;
using Dapper.Contrib.Extensions;
using System.Security.Cryptography;
using Redis.Commander.Utils;

namespace Redis.Commander.Data
{
    public class ConnectionRepository : IConnectionRepository
    {
        private readonly DbOptions _dbOptions;

        public ConnectionRepository(IOptions<DbOptions> options)
        {
            _dbOptions = options.Value;
        }

        public async Task<Connection> GetAsync(int connectionId)
        {
            using var db = new SqliteConnection(_dbOptions.ConnectionString);

            var sql = "SELECT * FROM Connection WHERE Id = @connectionId";

            var connection = await db.QuerySingleOrDefaultAsync<Connection>(sql, new { connectionId });

            if (connection != null)
            {
                connection.Pass = Crypto.Decrypt(connection.Pass);
            }

            return connection;
        }

        public async Task<ListConnectionsDto[]> ListConnectionsAsync()
        {
            using var db = new SqliteConnection(_dbOptions.ConnectionString);

            var sql = "SELECT Id, Name FROM Connection";

            var results = await db.QueryAsync<ListConnectionsDto>(sql);

            return results.ToArray();
        }

        public async Task<int> AddAsync(Connection connection)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            if (connection.Id > 0)
                throw new InvalidOperationException($"Unable to insert connection with non-zero id: {connection.Id}.");

            connection.Pass = Crypto.Encrypt(connection.Pass);

            using var db = new SqliteConnection(_dbOptions.ConnectionString);
            return await db.InsertAsync(connection);
        }

        public async Task UpdateAsync(Connection connection)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            if (connection.Id == 0)
                throw new InvalidOperationException($"Unable to update connection with zero id.");

            connection.Pass = Crypto.Encrypt(connection.Pass);

            using var db = new SqliteConnection(_dbOptions.ConnectionString);
            await db.UpdateAsync(connection);
        }
    }
}

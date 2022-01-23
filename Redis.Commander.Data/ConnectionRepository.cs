using System;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using Redis.Commander.Data.Contracts;
using Redis.Commander.Model;
using Redis.Commander.Model.DTOs;
using Dapper;
using System.Linq;

namespace Redis.Commander.Data
{
    public class ConnectionRepository : IConnectionRepository
    {
        private readonly DbOptions _dbOptions;

        public ConnectionRepository(IOptions<DbOptions> options)
        {
            _dbOptions = options.Value;
        }

        public async Task AddAsync(Connection connection)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            if (connection.Id > 0)
                throw new InvalidOperationException($"Unable to insert connection with non-zero id: {connection.Id}.");

            using var db = new SqliteConnection(_dbOptions.ConnectionString);

            var sql = @"
                INSERT INTO Connection (HostUrl, Port, User, Pass, UseSSL, Name)
                Values (@HostUrl, @Port, @User, @Pass, @UseSSL, @Name)
            ";

            await db.ExecuteScalarAsync(sql, connection);
        }

        public async Task<Connection> GetAsync(int connectionId)
        {
            using var db = new SqliteConnection(_dbOptions.ConnectionString);

            var sql = "SELECT * FROM Connection WHERE Id = @connectionId";

            var connection = await db.QuerySingleOrDefaultAsync<Connection>(sql, new { connectionId });

            return connection;
        }

        public async Task<ListConnectionsDto[]> ListConnectionsAsync()
        {
            using var db = new SqliteConnection(_dbOptions.ConnectionString);

            var sql = "SELECT Id, Name FROM Connection";

            var results = await db.QueryAsync<ListConnectionsDto>(sql);

            return results.ToArray();
        }

        public async Task UpdateAsync(Connection connection)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            if (connection.Id == 0)
                throw new InvalidOperationException($"Unable to update connection with zero id.");

            using var db = new SqliteConnection(_dbOptions.ConnectionString);

            var sql = @"
                UPDATE Connection SET
                    HostUrl = @HostUrl,
                    Port = @Port,
                    User = @User,
                    Pass = @Pass,
                    UseSSL = @UseSSL,
                    Name = @Name
                WHERE Id = @Id
            ";

            await db.ExecuteAsync(sql, connection);
        }
    }
}

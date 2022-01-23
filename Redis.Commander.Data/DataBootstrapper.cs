using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using Redis.Commander.Data.Contracts;
using Redis.Commander.Model;

namespace Redis.Commander.Data
{
    public class DataBootstrapper
    {
        private readonly DbOptions _dbOptions;

        public DataBootstrapper(IOptions<DbOptions> options)
        {
            _dbOptions = options.Value;
        }

        public async Task BootstrapAsync()
        {
            if (!File.Exists(_dbOptions.DatabaseName))
            {
                using var db = new SqliteConnection(_dbOptions.ConnectionString);

                // Do these sequentially so we don't run into any versioning or locking issues
                await CreateConnectionTableAsync(db);
                await CreateCommandTableAsync(db);
            }
        }

        private async Task CreateConnectionTableAsync(IDbConnection db)
        {
            if (await TableExistsAsync(db, nameof(Connection)))
                return;

            var sql = @"
                CREATE TABLE Connection (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    HostUrl TEXT,
                    Port INTEGER,
                    User TEXT,
                    Pass TEXT,
                    UseSSL INTEGER,
                    Name TEXT
                );
            ";

            await db.ExecuteAsync(sql);
        }

        private async Task CreateCommandTableAsync(IDbConnection db)
        {
            if (await TableExistsAsync(db, nameof(Command)))
                return;

            var sql = @"
                CREATE TABLE Command (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT,
                    Key TEXT,
                    Description TEXT
                );
            ";

            await db.ExecuteAsync(sql);
        }

        private async Task<bool> TableExistsAsync(IDbConnection db, string tableName)
        {
            var table = await db.QueryAsync<string>("SELECT name FROM sqlite_master WHERE type='table' AND name = '@tableName';", new
            {
                tableName
            });

            return table.Any();
        }
    }
}

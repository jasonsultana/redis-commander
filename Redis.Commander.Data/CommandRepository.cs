using System;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using Redis.Commander.Data.Contracts;
using Redis.Commander.Model;
using Redis.Commander.Model.DTOs;
using Dapper;
using System.Linq;
using Dapper.Contrib.Extensions;

namespace Redis.Commander.Data
{
    public class CommandRepository : ICommandRepository
    {
        private readonly DbOptions _dbOptions;

        public CommandRepository(IOptions<DbOptions> options)
        {
            _dbOptions = options.Value;
        }

        public async Task<int> AddAsync(Command command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (command.Id > 0)
                throw new InvalidOperationException($"Unable to insert command with non-zero id: {command.Id}");

            using var db = new SqliteConnection(_dbOptions.ConnectionString);
            return await db.InsertAsync(command);
        }

        public async Task<Command> GetAsync(int commandId)
        {
            using var db = new SqliteConnection(_dbOptions.ConnectionString);

            var sql = "SELECT * FROM Command WHERE Id = @commandId";
            var command = await db.QuerySingleOrDefaultAsync<Command>(sql, new { commandId });

            return command;
        }

        public async Task<ListCommandsDto[]> ListCommandsAsync()
        {
            using var db = new SqliteConnection(_dbOptions.ConnectionString);

            var sql = "SELECT Id, Name from Command";

            var results = await db.QueryAsync<ListCommandsDto>(sql);

            return results.ToArray();
        }

        public async Task UpdateAsync(Command command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (command.Id == 0)
                throw new InvalidOperationException("Unable to update command with zero id.");

            using var db = new SqliteConnection(_dbOptions.ConnectionString);
            await db.UpdateAsync(command);
        }
    }
}

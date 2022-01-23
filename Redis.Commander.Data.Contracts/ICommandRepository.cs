using System;
using System.Threading.Tasks;
using Redis.Commander.Model;
using Redis.Commander.Model.DTOs;

namespace Redis.Commander.Data.Contracts
{
    public interface ICommandRepository
    {
        Task AddAsync(Command command);

        Task UpdateAsync(Command command);

        Task<ListCommandsDto[]> ListCommandsAsync();

        Task<Command> GetAsync(int commandId);
    }
}

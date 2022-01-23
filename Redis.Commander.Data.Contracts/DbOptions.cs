using System;

namespace Redis.Commander.Data.Contracts
{
    public sealed class DbOptions
    {
        public string DatabaseName { get; set; } = string.Empty;

        public string ConnectionString => $"Data Source={DatabaseName}";
    }
}

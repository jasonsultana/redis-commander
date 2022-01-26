using System;
using Dapper.Contrib.Extensions;

namespace Redis.Commander.Model
{
    [Table("Connection")]
    public class Connection
    {
        public int Id { get; set; }

        public string HostUrl { get; set; } = string.Empty;

        public int Port { get; set; } = 6379;

        public string User { get; set; } = string.Empty;

        public string Pass { get; set; } = string.Empty;

        public bool UseSSL { get; set; } = true;

        public string Name { get; set; } = string.Empty;
    }
}

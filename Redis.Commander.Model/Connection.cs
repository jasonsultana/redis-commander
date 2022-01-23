using System;

namespace Redis.Commander.Model
{
    public class Connection
    {
        public int Id { get; set; }

        public string HostUrl { get; set; } = string.Empty;

        public int Port { get; set; }

        public string User { get; set; } = string.Empty;

        public string Pass { get; set; } = string.Empty;

        public bool UseSSL { get; set; } = true;

        public string Name { get; set; } = string.Empty;
    }
}

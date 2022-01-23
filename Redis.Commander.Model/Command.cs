using System;
namespace Redis.Commander.Model
{
    public class Command
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Key { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}

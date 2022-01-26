using System;
namespace Redis.Commander.Blazor.Shared.State
{
	public class AppState
	{
        public CommandState CommandState { get; set; } = new CommandState();

        public ConnectionState ConnectionState { get; set; } = new ConnectionState();

        public Alert Alert { get; set; } = Alert.Create();
    }
}
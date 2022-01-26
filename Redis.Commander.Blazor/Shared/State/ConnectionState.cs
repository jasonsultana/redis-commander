using System;
namespace Redis.Commander.Blazor.Shared.State
{
    public class ConnectionState : StateBase
    {
        private int? _connectionId;

        public int? ConnectionId
        {
            get => _connectionId;
            set
            {
                _connectionId = value;
                NotifyStateChanged();
            }
        }
    }
}

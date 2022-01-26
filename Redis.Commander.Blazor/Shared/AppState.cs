using System;
namespace Redis.Commander.Blazor.Shared
{
    public class ConnectionState
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

        public event Action OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}

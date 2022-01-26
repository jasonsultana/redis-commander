using System;
namespace Redis.Commander.Blazor.Shared.State
{
	public class CommandState : StateBase
	{
        private int? _commandId;

        public int? CommandId
        {
            get => _commandId;
            set
            {
                _commandId = value;
                NotifyStateChanged();
            }
        }
    }
}
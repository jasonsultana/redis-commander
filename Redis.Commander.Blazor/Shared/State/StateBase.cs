using System;
namespace Redis.Commander.Blazor.Shared.State
{
	public abstract class StateBase
	{
        public event Action OnChange;

        protected void NotifyStateChanged()
        {
            OnChange?.Invoke();
        }
    }
}


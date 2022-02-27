using System;
using Redis.Commander.Blazor.Shared.State;

namespace Redis.Commander.Blazor.Shared
{
    public class Alert : StateBase
    {
        private Alert(string @class, string message, bool visible = true)
        {
            this.StatusClass = @class;
            this.StatusMessage = message;
        }

        public bool IsVisible { get; private set; }

        public string StatusClass { get; private set; }

        public string StatusMessage { get; private set; }

        public static Alert Create() => new(@class: string.Empty, message: string.Empty, visible: false);

        public void Hide()
        {
            IsVisible = false;
            NotifyStateChanged();
        }

        public void ShowDanger(string message)
        {
            StatusClass = "alert-danger";
            StatusMessage = message;
            IsVisible = true;

            NotifyStateChanged();
        }

        public void ShowSuccess(string message)
        {
            StatusClass = "alert-success";
            StatusMessage = message;
            IsVisible = true;

            NotifyStateChanged();
        }

        public void ShowWarning(string message)
        {
            StatusClass = "alert-warning";
            StatusMessage = message;
            IsVisible = true;

            NotifyStateChanged();
        }
    }
}

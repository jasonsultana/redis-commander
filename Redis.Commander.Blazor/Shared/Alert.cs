using System;
namespace Redis.Commander.Blazor.Shared
{
    public class Alert
    {
        private Alert(string @class, string message)
        {
            this.StatusClass = @class;
            this.StatusMessage = message;
        }

        public string StatusClass { get; private set; }

        public string StatusMessage { get; private set; }

        public static Alert Danger(string message) => new Alert("alert-danger", message);

        public static Alert Success(string message) => new Alert("alert-success", message);
    }
}

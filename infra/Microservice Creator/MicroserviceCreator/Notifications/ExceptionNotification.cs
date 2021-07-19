using System;

namespace Cli.Notifications
{
    public class ExceptionNotification : INotification
    {
        public void Notify(string message)
        {
            throw new Exception(message);
        }
    }
}

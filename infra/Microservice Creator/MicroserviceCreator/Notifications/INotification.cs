using System;

namespace Cli.Notifications
{
    public interface INotification
    {
        public void Notify(String message);
    }
}

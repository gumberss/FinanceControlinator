using System;

namespace Cli.Notifications
{
    public class ConsoleNotification : INotification
    {
        public void Notify(string message)
        {
            Console.WriteLine(message);
        }
    }
}

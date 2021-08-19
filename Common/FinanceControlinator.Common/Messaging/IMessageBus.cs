using System.Threading;
using System.Threading.Tasks;

namespace FinanceControlinator.Common.Messaging
{
    public interface IMessageBus
    {
        Task Publish<T>(T message, CancellationToken cancellationToken = default) where T : class;
    }
}

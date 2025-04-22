using Application.Abstractions.Messaging;

namespace Infrastructure.Events;

internal sealed class EventBus(InMemoryMessageQueue queue) : IEventBus
{
    public async Task PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken)
        where T : class, IIntegrationEvent
    {
           await queue.Writer.WriteAsync(integrationEvent, cancellationToken);
    }
}

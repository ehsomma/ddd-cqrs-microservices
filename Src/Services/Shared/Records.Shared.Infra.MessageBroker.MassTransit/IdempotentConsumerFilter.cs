#pragma warning disable  // Disable all warnings

using System;
using System.Text.Json;
using System.Threading.Tasks;
using MassTransit;
using Records.Shared.Domain.Events;
using Records.Shared.Infra.Idempotence.Abstractions;
using Records.Shared.Infra.MessageBroker.Abstractions;

// (Obsolet), not used.
// Problem: We can't get the consumer namespace and class name in the Send() method, so
// we can't register it in redis to differentiate each consumer with the same event.
namespace Records.Shared.Infra.MessageBroker.MassTransit;

public class IdempotentConsumerFilter<T> : IFilter<ConsumeContext<T>>
    where T : class
{
    private readonly IIdempotentMessageService _idempotentMessageService;

    public IdempotentConsumerFilter(IServiceProvider dependency, IIdempotentMessageService idempotentMessageService)
    {
        _idempotentMessageService = idempotentMessageService;
    }

    public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    {
        await next.Send(context);

        /*
        Console.WriteLine($"[IDEMPOTENCY] IdempotentConsumerFilter");

        string messageTypeName = context.Message.GetType().Name;
        IIntegrationEvent message = context.Message as IIntegrationEvent;

        // NOTE: Tambien se puede usar context.MessageId de MassTransit.
        string messageId = message.MessageId;

        // Checks if the message already exist in the idempotency database/cache.
        if (!_idempotentMessageService.HasBeenProcessed(messageId, messageTypeName))
        {
            // If the message doesn't exist, sends the message, then persists it in the database/cache.
            Console.WriteLine($"[IDEMPOTENCY] =====> Consumed! messageTypeName: {messageTypeName}, messageId: {messageId}");

            // Sends the message and waits until has been processed.
            // If an exception occurs in the consumer, the process will stop here.
            await next.Send(context);

            // Persists it in the idempotency database/cache to avoid consume duplicate messages.
            await _idempotentMessageService.SaveAsync(messageId, messageTypeName);
        }
        else
        {
            Console.WriteLine($"[IDEMPOTENCY] =====X omited");
        }
        */
    }

    public void Probe(ProbeContext context)
    {
        context.CreateFilterScope("my-filter");
    }
}

#pragma warning restore  // Disable all warnings

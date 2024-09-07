using NATS.Client.JetStream;
using NATS.Client.JetStream.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class NatsService : INatsService
{
    private readonly NatsConnectionManager _connectionManager;

    public NatsService(NatsConnectionManager connectionManager)
    {
        _connectionManager = connectionManager ?? throw new ArgumentNullException(nameof(connectionManager));
    }

    public async Task PublishAsync(string streamName, string streamSubject, string message)
    {
        try
        {
            var js = _connectionManager.GetJetStreamContext();
            var ack = await js.PublishAsync(subject: streamSubject, data: message);
            ack.EnsureSuccess();
            Console.WriteLine($"Message published to {streamSubject}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error publishing message: {ex.Message}");
            throw;
        }
    }

    public async Task SubscribeAsync(string streamName, string consumerName, Func<string, Task> messageHandler)
    {
        try
        {
            var js = _connectionManager.GetJetStreamContext();

            // Create or update the consumer
            await js.CreateOrUpdateConsumerAsync(streamName, new ConsumerConfig(consumerName));

            // Get the consumer
            var consumer = await js.GetConsumerAsync(streamName, consumerName);

            // Consume messages
            await foreach (var msg in consumer.ConsumeAsync<string>())
            {
                await messageHandler(msg.Data);
                await msg.AckAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error subscribing to messages: {ex.Message}");
            throw;
        }
    }
}

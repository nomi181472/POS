using NATS.Client;
using NATS.Client.Core;
using NATS.Client.JetStream;
using NATS.Client.JetStream.Models;
using System;
using System.Text;
using System.Text.Json;

public class UserNotification
{
    public string UserId { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
}

public class NatsConsumer
{
    private static void Main(string[] args)
    {
        var nats = new NatsConnection();
        var js = new NatsJSContext(nats);
        var consumer = js.CreateOrUpdateConsumerAsync("orders", new ConsumerConfig("order_processor"));

        //foreach (var jsMsg in consumer.ConsumeAsync<string>())
        //{
        //    Console.WriteLine($"Processed: {jsMsg.Data}");
        //    jsMsg.AckAsync();
        //}
    }
}
using NATS.Client;
using NATS.Client.Core;
using NATS.Client.JetStream;
using System;
using System.Threading.Tasks;

public class NatsConnectionManager
{
    private NatsJSContext _jetStreamContext;

    public async Task ConnectAsync()
    {
        try
        {
            await using var nats = new NatsConnection();
            var js = new NatsJSContext(nats);

            Console.WriteLine("Connected to NATS JetStream.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error connecting to NATS JetStream: {ex.Message}");
            throw;
        }
    }

    public NatsJSContext GetJetStreamContext()
    {
        if (_jetStreamContext == null)
        {
            throw new InvalidOperationException("Not connected to NATS JetStream.");
        }

        return _jetStreamContext;
    }
}

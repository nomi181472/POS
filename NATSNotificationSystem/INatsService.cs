using System.Threading.Tasks;

public interface INatsService
{
    Task PublishAsync(string streamName, string streamSubject, string message);
    Task SubscribeAsync(string streamName, string consumerName, Func<string, Task> messageHandler);
}

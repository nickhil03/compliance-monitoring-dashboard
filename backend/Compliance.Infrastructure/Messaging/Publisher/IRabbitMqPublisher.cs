namespace Compliance.Infrastructure.Messaging.Publisher;
public interface IRabbitMqPublisher
{
    public Task Publish(string queueName, object message);
}
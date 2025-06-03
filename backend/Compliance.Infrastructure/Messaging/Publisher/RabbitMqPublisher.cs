using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Compliance.Infrastructure.Messaging.Publisher;
public class RabbitMqPublisher(IConnectionFactory connectionFactory) : IRabbitMqPublisher
{
    async Task IRabbitMqPublisher.Publish(string queueName, object message)
    {
        using var connection = await connectionFactory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();
        await channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        var props = new BasicProperties { Persistent = true };

        await channel.BasicPublishAsync(
            exchange: "",
            routingKey: queueName,
            mandatory: false,
            basicProperties: props,
            body: body
        );
    }
}

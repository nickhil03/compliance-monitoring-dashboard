using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Compliance.Application.Commands.Evaluate.Command;

namespace Compliance.Infrastructure.Messaging.Consumer
{
    public class RabbitMqConsumerService(IConnectionFactory _connectionFactory, IServiceProvider serviceProvider) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            const string queueName = "compliance-evaluation-queue";
            var connection = await _connectionFactory.CreateConnectionAsync(stoppingToken);
            using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);
            await channel.QueueDeclareAsync(queueName, true, false, false, cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (sender, ea) =>
            {
                var body = ea.Body.ToArray();
                var command = JsonSerializer.Deserialize<EvaluateComplianceCommand>(Encoding.UTF8.GetString(body));

                if (command != null)
                {
                    using var scope = serviceProvider.CreateScope();
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    await mediator.Send(command, stoppingToken);
                }

                await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
            };

            await channel.BasicConsumeAsync(queueName, false, consumer, stoppingToken);
        }
    }
}
using Compliance.Application.Commands.Evaluate.Command;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Compliance.Worker.Services
{
    public class ComplianceEvaluationConsumer(ILogger<ComplianceEvaluationConsumer> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            const string queueName = "compliance-evaluation-queue";
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = await factory.CreateConnectionAsync(stoppingToken);
            using var channel = await connection.CreateChannelAsync(null, stoppingToken);

            await channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = JsonSerializer.Deserialize<EvaluateComplianceCommand>(Encoding.UTF8.GetString(body));

                // Evaluate compliance logic here (fetch rules, check data, store result)
                Console.WriteLine($"[Worker] Evaluating RuleID: {message?.RuleId}");

                await Task.CompletedTask;
            };

            await channel.BasicConsumeAsync(queueName, true, consumer, stoppingToken);
        }
    }

}

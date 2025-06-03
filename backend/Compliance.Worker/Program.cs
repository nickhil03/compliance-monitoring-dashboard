using Compliance.Worker.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<ComplianceEvaluationConsumer>();

var host = builder.Build();
host.Run();
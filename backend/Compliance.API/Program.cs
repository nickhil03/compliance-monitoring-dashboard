using Compliance.API.Middleware;
using Compliance.Application.Queries.Query.ComplianceResultQuery;
using Compliance.Application.Validators;
using Compliance.Domain.Repositories.Activity;
using Compliance.Domain.Repositories.ComplianceItemsRepos;
using Compliance.Domain.Repositories.ComplianceRule;
using Compliance.Domain.Repositories.ComplianceStatusRepos;
using Compliance.Domain.Settings;
using Compliance.Infrastructure.Messaging.Consumer;
using Compliance.Infrastructure.Messaging.Publisher;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();

// Configure CORS to allow all origins, methods, and headers
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configure Swagger and OpenAPI
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "Compliance API", Version = "v1" });

    // Add JWT Authentication to Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your token.\n\nExample: **Bearer eyJhbGci...**"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// MongoDB Config Binding
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDB")
);

builder.Services.AddSingleton<IMongoClient>(sp =>
    new MongoClient(sp.GetRequiredService<IOptions<MongoDbSettings>>().Value.ConnectionString)
);

builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IMongoClient>().GetDatabase(
        sp.GetRequiredService<IOptions<MongoDbSettings>>().Value.Database)
);

builder.Services.AddScoped<IComplianceRuleRepository>(sp =>
    new ComplianceRuleRepository(
        sp.GetRequiredService<IMongoDatabase>()
    )
);

builder.Services.AddScoped<IRecentActivitiesRepository>(sp =>
    new RecentActivitiesRepository(
        sp.GetRequiredService<IMongoDatabase>()
    )
);

builder.Services.AddScoped<IComplianceItemsRepository>(sp =>
    new ComplianceItemsRepository(
        sp.GetRequiredService<IMongoDatabase>()
    )
);

builder.Services.AddScoped<IComplianceStatusRepository>(sp =>
    new ComplianceStatusRepository(
        sp.GetRequiredService<IMongoDatabase>()
    )
);

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(GetAllComplianceResultsQuery).Assembly);
});

//builder.Services.AddSingleton(sp =>
//{
//    var factory = new ConnectionFactory
//    {
//        HostName = "localhost",
//        AutomaticRecoveryEnabled = true
//    };
//    return (IConnectionFactory)factory.CreateConnectionAsync().GetAwaiter().GetResult();
//});

//builder.Services.AddSingleton<IRabbitMqPublisher, RabbitMqPublisher>();
//builder.Services.AddHostedService<RabbitMqConsumerService>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateComplianceResultCommandValidator>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins");
app.UseRouting();

// Custom authentication middleware
app.UseTokenValidation();
app.UseAuthorization();

app.MapControllers();
app.Run();

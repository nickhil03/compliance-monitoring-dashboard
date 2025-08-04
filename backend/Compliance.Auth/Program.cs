using Compliance.Auth.ValidationLogic;
using Compliance.Domain.Repositories.Users;
using Compliance.Domain.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add Serilog configuration
//builder.Host.UseSerilog((context, services, configuration) =>
//    configuration
//        .ReadFrom.Configuration(context.Configuration)
//        .ReadFrom.Services(services)
//        .Enrich.FromLogContext()
//);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
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
builder.Services.AddScoped<AuthenticationService>();

builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDB"));

builder.Services.AddSingleton<IMongoClient>(sp =>
    new MongoClient(sp.GetRequiredService<IOptions<MongoDbSettings>>().Value.ConnectionString)
);

builder.Services.AddScoped<IUserRepository>(sp =>
    new UserRepository(sp.GetRequiredService<IMongoClient>().GetDatabase(
        sp.GetRequiredService<IOptions<MongoDbSettings>>().Value.Database
        ))
);

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAllOrigins");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

using Compliance.Auth.ValidationLogic.Contracts;
using Compliance.Auth.ValidationLogic.Services;
using Compliance.Domain.Repositories.TokenModel.RefreshTokenRepos;
using Compliance.Domain.Repositories.UsersRepos;
using Compliance.Domain.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
//using Serilog;

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
    options.AddPolicy("AllowFrontEnd",
        builder =>
        {
            builder.WithOrigins("http://localhost:5173")
            .AllowCredentials()
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
});
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ITokenLogic, TokenLogic>();

builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDB"));

builder.Services.AddSingleton<IMongoClient>(sp =>
    new MongoClient(sp.GetRequiredService<IOptions<MongoDbSettings>>().Value.ConnectionString)
);

builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IMongoClient>().GetDatabase(
        sp.GetRequiredService<IOptions<MongoDbSettings>>().Value.Database
        )
);

builder.Services.AddScoped<IUserRepository>(sp =>
    new UserRepository(sp.GetRequiredService<IMongoDatabase>())
);

builder.Services.AddScoped<IRefreshTokenRepository>(sp =>
    new RefreshTokenRepository(sp.GetRequiredService<IMongoDatabase>())
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

app.UseCors("AllowFrontEnd");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

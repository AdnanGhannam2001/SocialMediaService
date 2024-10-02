using SocialMediaService.Persistent.Extensions;
using SocialMediaService.Application.Extensions;
using SocialMediaService.WebApi.Extensions;
using SocialMediaService.WebApi.Implementaions;
using SocialMediaService.WebApi.JsonConverters;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using SocialMediaService.Persistent.Data.Seed;
using SocialMediaService.Persistent.Data;
using SocialMediaService.Infrastructure.Extensions;
using MassTransit;
using SocialMediaService.WebApi.Configurations;
using SocialMediaService.Infrastructure.Constants;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(opt => 
    {
        opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        opt.JsonSerializerOptions.Converters.Add(new ExceptionBaseConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<Storage>(builder.Configuration.GetSection("Storage"));

builder.Services
#if DEBUG && !NO_RABBIT_MQ
    .AddInfrastructure()
#endif //DEBUG && !NO_RABBIT_MQ
    .AddPersistent(builder.Configuration.GetConnectionString(DatabaseConstants.ConnectionStringName))
    .AddApplication()
    .AddAuth(builder.Configuration.GetSection(nameof(OpenIdConnectOptions)))
    .RegisterServices()
    .AddCors()
    .AddGrpc();

var app = builder.Build();

if (args.Contains("--seed"))
{
    using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var messagePublisher = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
    SeedData.ApplyAsync(context, messagePublisher).GetAwaiter().GetResult();
    return;
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(x =>
{
    x
        .SetIsOriginAllowed(x => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcService<ProfileServiceImpl>();

app.MapControllers();

app.Run();
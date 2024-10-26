using SocialMediaService.Persistent.Extensions;
using SocialMediaService.Application.Extensions;
using SocialMediaService.WebApi.Extensions;
using SocialMediaService.WebApi.Implementaions;
using SocialMediaService.WebApi.JsonConverters;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using SocialMediaService.Infrastructure.Extensions;
using SocialMediaService.WebApi.Configurations;
using SocialMediaService.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;

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
#if !NO_RABBIT_MQ
    .AddInfrastructure(builder.Configuration.GetSection(CommonConstants.RabbitMqSettings))
#endif // !NO_RABBIT_MQ
    .AddPersistent(builder.Configuration.GetConnectionString(CommonConstants.ConnectionStringName))
    .AddApplication()
    .AddAuth(builder.Configuration.GetSection(nameof(OpenIdConnectOptions)))
    .RegisterServices()
    .AddCors()
    .AddGrpc();

var app = builder.Build();

app.Setup();
app.HandleArguments(args);

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
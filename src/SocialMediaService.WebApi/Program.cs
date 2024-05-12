using SocialMediaService.Persistent.Extensions;
using SocialMediaService.Application.Extensions;
using SocialMediaService.WebApi.Extensions;
using SocialMediaService.WebApi.Implementaions;
using SocialMediaService.WebApi.JsonConverters;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");

builder.Services.AddControllers()
    .AddJsonOptions(opt => 
    {
        opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        opt.JsonSerializerOptions.Converters.Add(new ExceptionBaseConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddPersistent(connectionString)
    .AddApplication()
    .AddAuth()
    .AddGrpc();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcService<ProfileServiceImpl>();

app.MapControllers();

app.Run();
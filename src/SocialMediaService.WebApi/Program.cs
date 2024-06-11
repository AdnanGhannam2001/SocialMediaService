using SocialMediaService.Persistent.Extensions;
using SocialMediaService.Application.Extensions;
using SocialMediaService.WebApi.Extensions;
using SocialMediaService.WebApi.Implementaions;
using SocialMediaService.WebApi.JsonConverters;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using SocialMediaService.Persistent.Data.Seed;
using SocialMediaService.Persistent.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(opt => 
    {
        opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        opt.JsonSerializerOptions.Converters.Add(new ExceptionBaseConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddPersistent(builder.Configuration.GetConnectionString("PostgresConnection"))
    .AddApplication()
    .AddAuth(builder.Configuration.GetSection(nameof(OpenIdConnectOptions)))
    .AddGrpc();

var app = builder.Build();

if (args.Contains("--seed"))
{
    using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    SeedData.ApplyAsync(context).GetAwaiter().GetResult();
    return;
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

#if DEBUG
app.UseCors(x =>
{
    x
        .SetIsOriginAllowed(x => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
});
#endif

app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcService<ProfileServiceImpl>();

app.MapControllers();

app.Run();
using MeCorp.Y.Application;
using MeCorp.Y.Infrastructure.Data;
using MeCorp.Y.Infrastructure.Security;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(jsonOptions => jsonOptions.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddOpenApi();
builder.Services.AddApplicationLayerServices();
builder.Services.AddDataServices(builder.Configuration);
builder.Services.AddSecurityServices(builder.Configuration);
builder.Services.AddRateLimiterPolicies();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.Services.InitializeDatabase();

app.UseHttpsRedirection();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

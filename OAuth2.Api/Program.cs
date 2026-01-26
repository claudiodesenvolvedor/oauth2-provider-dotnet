using OAuth2.Api;
using OAuth2.Application;
using OAuth2.Infrastructure;
using OAuth2.Presentation;
using OAuth2.Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddPresentation();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();
builder.Services.AddSingleton<IConfigureOptions<JwtBearerOptions>, JwtBearerOptionsSetup>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("admin", policy =>
        policy.RequireAssertion(context =>
        {
            var scope = context.User.FindFirst("scope")?.Value;
            if (string.IsNullOrWhiteSpace(scope))
            {
                return false;
            }

            return scope
                .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Any(s => string.Equals(s, "admin", StringComparison.Ordinal));
        }));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program
{
}

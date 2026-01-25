using OAuth2.Application;
using OAuth2.Infrastructure;
using OAuth2.Presentation;
using OAuth2.Application.Interfaces.Security;
using OAuth2.Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddPresentation();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration
            .GetSection(JwtSettings.SectionName)
            .Get<JwtSettings>() ?? new JwtSettings();

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1),
            ValidateIssuerSigningKey = true,
            IssuerSigningKeyResolver = (token, securityToken, kid, parameters) =>
            {
                using var scope = builder.Services.BuildServiceProvider().CreateScope();
                var keyProvider = scope.ServiceProvider.GetRequiredService<IJwtKeyProvider>();
                var jwks = keyProvider.GetJwksAsync(CancellationToken.None)
                    .GetAwaiter()
                    .GetResult();

                return jwks.Keys
                    .Where(key => string.IsNullOrWhiteSpace(kid) || key.Kid == kid)
                    .Select(BuildRsaSecurityKey)
                    .ToList();
            }
        };
    });
builder.Services.AddAuthorization();
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

static SecurityKey BuildRsaSecurityKey(OAuth2.Application.DTOs.Security.JwksKey key)
{
    var parameters = new RSAParameters
    {
        Modulus = Base64UrlDecode(key.N),
        Exponent = Base64UrlDecode(key.E)
    };

    return new RsaSecurityKey(parameters) { KeyId = key.Kid };
}

static byte[] Base64UrlDecode(string value)
{
    var padding = value.Length % 4;
    if (padding > 0)
    {
        value = value + new string('=', 4 - padding);
    }

    value = value.Replace('-', '+').Replace('_', '/');
    return Convert.FromBase64String(value);
}

public partial class Program
{
}

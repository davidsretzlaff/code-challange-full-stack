using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeManagement.API.Middleware
{
    public sealed class JwtAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<JwtAuthMiddleware> _logger;
        private readonly IConfiguration _configuration;

        public JwtAuthMiddleware(RequestDelegate next, ILogger<JwtAuthMiddleware> logger, IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            if (ShouldSkip(context))
            {
                await _next(context);
                return;
            }

            var authorizationHeader = context.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrWhiteSpace(authorizationHeader) || !authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context); // Deixa [Authorize] decidir 401 quando necessário
                return;
            }

            var token = authorizationHeader["Bearer ".Length..].Trim();
            if (string.IsNullOrWhiteSpace(token))
            {
                await WriteUnauthorizedAsync(context, "Token ausente.");
                return;
            }

            try
            {
                var principal = ValidateToken(token);
                if (principal is null)
                {
                    await WriteUnauthorizedAsync(context, "Token inválido.");
                    return;
                }

                context.User = principal;
                await _next(context);
            }
            catch (SecurityTokenException ex)
            {
                _logger.LogWarning(ex, "Falha de validação de token JWT");
                await WriteUnauthorizedAsync(context, "Token inválido.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado durante autenticação JWT");
                await WriteUnauthorizedAsync(context, "Falha ao autenticar o token.");
            }
        }

        private ClaimsPrincipal? ValidateToken(string token)
        {
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var rawSecret = _configuration["Jwt:Secret"] ?? "dev-secret-change-me-32-bytes-minimum-change-me";
            var normalizedSecret = EnsureMinKeyLength(rawSecret, 32);

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(normalizedSecret)),
                ValidateIssuer = !string.IsNullOrWhiteSpace(issuer),
                ValidIssuer = issuer,
                ValidateAudience = !string.IsNullOrWhiteSpace(audience),
                ValidAudience = audience,
                ClockSkew = TimeSpan.FromSeconds(30)
            };

            return tokenHandler.ValidateToken(token, validationParameters, out _);
        }

        private static string EnsureMinKeyLength(string? input, int minBytes)
        {
            var value = input ?? string.Empty;
            var builder = new StringBuilder(value);
            while (Encoding.UTF8.GetByteCount(builder.ToString()) < minBytes)
            {
                builder.Append('0');
            }
            return builder.ToString();
        }

        private static bool ShouldSkip(HttpContext context)
        {
            var path = context.Request.Path.Value ?? string.Empty;
            if (path.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase)) return true;
            if (path.Equals("/", StringComparison.Ordinal)) return true;
            if (path.Equals("/api/auth/login", StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }

        private static async Task WriteUnauthorizedAsync(HttpContext context, string message)
        {
            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                var payload = new { title = "Não autorizado", status = 401, detail = message };
                await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
            }
        }
    }
}



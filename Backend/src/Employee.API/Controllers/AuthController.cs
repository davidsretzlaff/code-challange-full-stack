using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public sealed record LoginRequest(
            [property: JsonPropertyName("email")] string Email,
            [property: JsonPropertyName("password")] string Password
        );
        public sealed record LoginResponse(string AccessToken, int ExpiresInSeconds);

        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (!IsValidUser(request))
            {
                return Unauthorized();
            }

            var token = GenerateMockJwtToken();
            return Ok(token);
        }

        private static bool IsValidUser(LoginRequest request)
        {
            return string.Equals(request.Email, "admin@admin.com", StringComparison.OrdinalIgnoreCase)
                && string.Equals(request.Password, "123456", StringComparison.Ordinal);
        }

        private LoginResponse GenerateMockJwtToken()
        {
            var issuer = _configuration["Jwt:Issuer"]; // optional
            var audience = _configuration["Jwt:Audience"]; // optional
            var rawSecret = _configuration["Jwt:Secret"] ?? "dev-secret-change-me-32-bytes-minimum-change-me"; // fallback for local
            var normalizedSecret = EnsureMinKeyLength(rawSecret, 32);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(normalizedSecret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, "admin@admin.com"),
                new Claim(JwtRegisteredClaimNames.Email, "admin@admin.com"),
                new Claim(ClaimTypes.Name, "admin"),
                new Claim(ClaimTypes.Email, "admin@admin.com"),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var expires = DateTime.UtcNow.AddHours(1);

            var token = new JwtSecurityToken(
                issuer: string.IsNullOrWhiteSpace(issuer) ? null : issuer,
                audience: string.IsNullOrWhiteSpace(audience) ? null : audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expires,
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return new LoginResponse(tokenString, (int)TimeSpan.FromHours(1).TotalSeconds);
        }

        private static string EnsureMinKeyLength(string? input, int minBytes)
        {
            var value = input ?? string.Empty;
            // Evita exceção IDX10720 garantindo ao menos 256 bits (32 bytes)
            var builder = new StringBuilder(value);
            while (Encoding.UTF8.GetByteCount(builder.ToString()) < minBytes)
            {
                builder.Append('0');
            }
            return builder.ToString();
        }
    }
}



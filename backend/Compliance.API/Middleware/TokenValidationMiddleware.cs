using System.Text.Json;

namespace Compliance.API.Middleware
{
    public class TokenValidationMiddleware(RequestDelegate _next, IHttpClientFactory _httpClientFactory, IConfiguration _configuration)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/api") && !context.Request.Path.StartsWithSegments("/api/Auth"))
            {
                string? authorizationHeader = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").LastOrDefault();

                if (!string.IsNullOrEmpty(authorizationHeader))
                {
                    using var client = _httpClientFactory.CreateClient();
                    var authServiceUrl = _configuration["AuthServiceUrl"];

                    var response = await client.PostAsJsonAsync($"{authServiceUrl}/api/Auth/validate-token", new { Token = authorizationHeader });

                    if (response.IsSuccessStatusCode)
                    {
                        var validationResult = await response.Content.ReadFromJsonAsync<JsonDocument>();
                        if (validationResult?.RootElement.GetProperty("isValid").GetBoolean() == true)
                        {
                            var claims = validationResult.RootElement.GetProperty("claims").EnumerateArray()
                                .Select(claim => new System.Security.Claims.Claim(claim.GetProperty("Type").GetString()!, claim.GetProperty("Value").GetString()!));

                            var identity = new System.Security.Claims.ClaimsIdentity(claims, "Custom");
                            context.User = new System.Security.Claims.ClaimsPrincipal(identity);

                            await _next(context); // Token is valid, proceed to the API endpoint
                            return;
                        }
                    }

                    context.Response.StatusCode = 401; // Unauthorized
                    await context.Response.WriteAsync("Unauthorized");
                    return;
                }
                else
                {
                    context.Response.StatusCode = 401; // Unauthorized
                    await context.Response.WriteAsync("Unauthorized");
                    return;
                }
            }

            await _next(context); // For non-API routes or Auth controller, just proceed
        }
    }
}

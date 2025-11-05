using System.Security.Claims;
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
                    var authServiceUrl = _configuration["AuthServiceUrl"];

                    var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{authServiceUrl}api/Auth/validate")
                    {
                        Content = JsonContent.Create(new { Token = authorizationHeader })
                    };

                    // Forward cookies from the incoming request
                    if (context.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
                    {
                        requestMessage.Headers.Add("Cookie", $"refreshToken={refreshToken}");
                    }

                    using var client = _httpClientFactory.CreateClient();
                    var response = await client.SendAsync(requestMessage);

                    if (response.IsSuccessStatusCode)
                    {
                        var validationResult = await response.Content.ReadFromJsonAsync<JsonDocument>();
                        if (validationResult?.RootElement.GetProperty("isValid").GetBoolean() == true)
                        {
                            var claims = validationResult.RootElement.GetProperty("claims").EnumerateArray()
                                .Select(claim => new Claim(claim.GetProperty("type").GetString()!, claim.GetProperty("value").GetString()!));

                            var identity = new ClaimsIdentity(claims, "Bearer");
                            context.User = new ClaimsPrincipal(identity);

                            await _next(context); // Token is valid, proceed to the API endpoint
                            return;
                        }
                    }

                    context.Response.StatusCode = 401; // Unauthorized
                    await context.Response.WriteAsync("Unauthorized");
                    return;// Short-Circuited to return 401
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

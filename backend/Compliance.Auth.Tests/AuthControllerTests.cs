using System.Security.Claims;
using Compliance.Auth.Controllers;
using Compliance.Auth.ValidationLogic.Contracts;
using Compliance.Domain.Model;
using Compliance.Domain.Repositories.TokenModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Compliance.Auth.Tests
{
    public class AuthControllerTests
    {
        private class TestRequestCookies(Dictionary<string, string> values) : IRequestCookieCollection
        {
            public string? this[string key] => values.TryGetValue(key, out var v) ? v : null;
            public int Count => values.Count;
            public ICollection<string> Keys => values.Keys;
            public bool ContainsKey(string key) => values.ContainsKey(key);
            public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => values.GetEnumerator();
            public bool TryGetValue(string key, out string value) => values.TryGetValue(key, out value!);
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => values.GetEnumerator();
        }

        private readonly Mock<IAuthenticationService> _authService = new();
        private readonly Mock<ITokenLogic> _tokenLogic = new();

        private AuthController CreateController()
        {
            var controller = new AuthController(_authService.Object, _tokenLogic.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
            return controller;
        }

        [Fact]
        public async Task Login_Returns_Unauthorized_When_Invalid_Credentials()
        {
            var controller = CreateController();
            var model = new UserLoginModel { Username = "u", Password = "p" };
            _authService.Setup(s => s.AuthenticateUserAsync(model.Username, model.Password)).ReturnsAsync(true);

            var result = await controller.Login(model);

            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task Login_Returns_Ok_And_Sets_Cookie_On_Success()
        {
            var controller = CreateController();
            var model = new UserLoginModel { Username = "u", Password = "p" };
            _authService.Setup(s => s.AuthenticateUserAsync(model.Username, model.Password)).ReturnsAsync(false);

            var token = new TokenModel
            {
                AccessToken = "at",
                RefreshToken = "rt",
                AccessTokenExpires = DateTime.UtcNow.AddMinutes(30),
                RefreshTokenExpires = DateTime.UtcNow.AddDays(7)
            };

            _tokenLogic.Setup(t => t.GenerateTokens(model.Username, It.IsAny<string>(), It.IsAny<string>(), false)).ReturnsAsync(token);

            var result = await controller.Login(model);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(token, ok.Value);
            Assert.True(controller.Response.Headers.ContainsKey("Set-Cookie"));
        }

        [Fact]
        public async Task Register_Returns_BadRequest_When_Fails()
        {
            var controller = CreateController();
            var model = new UserRegisterModel { Username = "u", Name = "n", Password = "p" };
            _authService.Setup(s => s.RegisterUserAsync(model)).ReturnsAsync(false);

            var result = await controller.Register(model);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Register_Returns_Ok_On_Success()
        {
            var controller = CreateController();
            var model = new UserRegisterModel { Username = "u", Name = "n", Password = "p" };
            _authService.Setup(s => s.RegisterUserAsync(model)).ReturnsAsync(true);

            var result = await controller.Register(model);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void ValidateToken_Returns_BadRequest_When_Missing()
        {
            var controller = CreateController();
            controller.Request.Cookies = new TestRequestCookies([]);
            var req = new TokenRequest { Token = null };

            var result = controller.ValidateToken(req);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void ValidateToken_Returns_Unauthorized_When_Invalid()
        {
            var controller = CreateController();
            controller.Request.Cookies = new TestRequestCookies(new Dictionary<string, string> { { "refreshToken", "rt" } });
            var req = new TokenRequest { Token = "t" };
            _tokenLogic.Setup(t => t.ValidateJwtToken(req.Token)).Returns((ClaimsPrincipal?)null);

            var result = controller.ValidateToken(req);

            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public void ValidateToken_Returns_Ok_When_Valid()
        {
            var controller = CreateController();
            controller.Request.Cookies = new TestRequestCookies(new Dictionary<string, string> { { "refreshToken", "rt" } });
            var req = new TokenRequest { Token = "t" };
            var claims = new[] { new Claim("sub", "1") };
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);
            _tokenLogic.Setup(t => t.ValidateJwtToken(req.Token)).Returns(principal);

            var result = controller.ValidateToken(req);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(ok.Value);
        }

        [Fact]
        public async Task RefreshToken_Returns_BadRequest_When_No_Cookie()
        {
            var controller = CreateController();
            controller.Request.Cookies = new TestRequestCookies([]);

            var result = await controller.RefreshToken();

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task RefreshToken_Returns_Unauthorized_When_Invalid_RefreshToken()
        {
            var controller = CreateController();
            controller.Request.Cookies = new TestRequestCookies(new Dictionary<string, string> { { "refreshToken", "rt" } });
            _tokenLogic.Setup(t => t.GetUserNameFromRefreshTokenAsync("rt")).ReturnsAsync((string?)null);

            var result = await controller.RefreshToken();

            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task RefreshToken_Returns_Ok_On_Success()
        {
            var controller = CreateController();
            controller.Request.Cookies = new TestRequestCookies(new Dictionary<string, string> { { "refreshToken", "rt" } });
            _tokenLogic.Setup(t => t.GetUserNameFromRefreshTokenAsync("rt")).ReturnsAsync("u");

            var newToken = new TokenModel
            {
                AccessToken = "at",
                RefreshToken = "rt2",
                AccessTokenExpires = DateTime.UtcNow.AddMinutes(30),
                RefreshTokenExpires = DateTime.UtcNow.AddDays(7)
            };

            _tokenLogic.Setup(t => t.GenerateTokens("u", It.IsAny<string>(), It.IsAny<string>(), true)).ReturnsAsync(newToken);
            _tokenLogic.Setup(t => t.RevokeRefreshTokenAsync("rt", newToken.RefreshToken)).Returns(Task.CompletedTask);

            var result = await controller.RefreshToken();

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(newToken, ok.Value);
        }

        [Fact]
        public async Task Logout_Deletes_Cookie_And_Returns_Ok()
        {
            var controller = CreateController();
            controller.Request.Cookies = new TestRequestCookies(new Dictionary<string, string> { { "refreshToken", "rt" } });
            _tokenLogic.Setup(t => t.RevokeRefreshTokenAsync("rt")).Returns(Task.CompletedTask);

            var result = await controller.Logout();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task RevokeAllTokens_Calls_TokenLogic_With_UserId()
        {
            var controller = CreateController();
            var claims = new List<Claim> { new("_id", "123") };
            controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(claims));
            _tokenLogic.Setup(t => t.RevokeAllUserTokensAsync("123")).Returns(Task.CompletedTask).Verifiable();

            var result = await controller.RevokeAllTokens();

            Assert.IsType<OkObjectResult>(result);
            _tokenLogic.Verify();
        }
    }
}

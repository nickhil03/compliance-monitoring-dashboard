using Compliance.Auth.ValidationLogic.Contracts;
using Compliance.Domain.Model;
using Compliance.Domain.Repositories.RefreshTokenRepos;
using Compliance.Domain.Repositories.UsersRepos;
using System.Linq.Expressions;

namespace Compliance.Auth.ValidationLogic.Services
{
    public class AuthenticationService(IUserRepository _userRepo, IRefreshTokenRepository _refreshTokenRepo) : IAuthenticationService
    {
        public async Task<bool> AuthenticateUserAsync(string username, string password)
        {
            return await _userRepo.ValidateUserCredentialsAsync(username, password);            
        }

        public async Task<List<RefreshToken>> GetRefreshTokensAsync(params Expression<Func<RefreshToken, bool>>[] predicates)
        {
            return await _refreshTokenRepo.GetNonRevokedRefreshTokensAsync(predicates);            
        }

        public async Task<bool> RegisterUserAsync(UserRegisterModel userModel)
        {
            if (await _userRepo.GetUserByUsernameAsync(userModel.Username) != null)
            {
                return false; // User already exists  
            }

            var user = new User
            {
                _id = new MongoDB.Bson.ObjectId(), // Generate a unique ID  
                Name = userModel.Name,
                Username = userModel.Username,
                Email = userModel.Email,
                Roles = userModel.Roles,
                Password = BCrypt.Net.BCrypt.HashPassword(userModel.Password)
            };

            await _userRepo.CreateUserAsync(user);
            return true;
        }

        public async Task SaveRefreshTokenAsync(RefreshToken refreshTokenEntity)
        {
            refreshTokenEntity.Token = BCrypt.Net.BCrypt.HashPassword(refreshTokenEntity.Token);
            refreshTokenEntity.IsUsed = true;
            await _refreshTokenRepo.SaveRefreshTokenAsync(refreshTokenEntity);
        }

        public async Task UpdateRefreshTokenAsync(RefreshToken refreshToken)
        {
            await _refreshTokenRepo.UpdateRefreshTokenAsync(refreshToken);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _userRepo.GetUserByUsernameAsync(username);
        }
    }
}
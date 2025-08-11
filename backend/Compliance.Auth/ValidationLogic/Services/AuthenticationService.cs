using Compliance.Auth.ValidationLogic.Contracts;
using Compliance.Domain.Model;
using Compliance.Domain.Repositories.RefreshTokenRepos;
using Compliance.Domain.Repositories.UsersRepos;

namespace Compliance.Auth.ValidationLogic.Services
{
    public class AuthenticationService(IUserRepository _userRepo, IRefreshTokenRepository _refreshTokenRepo) : IAuthenticationService
    {
        public async Task<User?> AuthenticateUserAsync(string username, string password)
        {
            var user = await _userRepo.GetUserByUsernameAsync(username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return null;
            }
            return user;
        }

        public async Task<bool> RegisterUserAsync(UserRegisterModel userModel)
        {
            if (await _userRepo.GetUserByUsernameAsync(userModel.Username) != null)
            {
                return false; // User already exists  
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userModel.Password);
            var user = new User
            {
                _id = new MongoDB.Bson.ObjectId(), // Generate a unique ID  
                Name = userModel.Name,
                Username = userModel.Username,
                Email = userModel.Email,
                Roles = userModel.Roles,
                Password = hashedPassword
            };

            await _userRepo.CreateUserAsync(user);
            return true;
        }

        public async Task SaveRefreshTokenAsync(RefreshToken refreshTokenEntity)
        {
            refreshTokenEntity.Token = BCrypt.Net.BCrypt.HashPassword(refreshTokenEntity.Token); // Generate a new token
            refreshTokenEntity.IsUsed = true;
            // Save the refresh token to the database
            await _refreshTokenRepo.SaveRefreshTokenAsync(refreshTokenEntity);            
        }

        public async Task<bool> ValidateRefreshTokenAsync(string refreshToken)
        {
            // Assuming you have a method in your repository to validate the refresh token
            // return await _userRepo.ValidateRefreshTokenAsync(refreshToken);
            // For now, we will just simulate validation
            return BCrypt.Net.BCrypt.Verify(refreshToken, "storedHashedRefreshToken"); // Replace with actual stored token
        }
    }
}
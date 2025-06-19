using Compliance.Domain.Model;
using Compliance.Domain.Repositories.Users;

namespace Compliance.Auth.ValidationLogic
{
    public class AuthenticationService(IUserRepository _userRepo)
    {
        public async Task<User?> AuthenticateAsync(string username, string password)
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
    }
}
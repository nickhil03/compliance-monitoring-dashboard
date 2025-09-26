using Compliance.Domain.Model;

namespace Compliance.Domain.Repositories.UsersRepos;
public interface IUserRepository
{
    Task<bool> ValidateUserCredentialsAsync(string username, string password);
    Task<User?> GetUserByUsernameAsync(string username);
    Task CreateUserAsync(User user);
}

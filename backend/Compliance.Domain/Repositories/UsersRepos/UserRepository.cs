using Compliance.Domain.Model;
using MongoDB.Driver;

namespace Compliance.Domain.Repositories.UsersRepos
{
    public class UserRepository(IMongoDatabase database) : IUserRepository
    {
        private readonly IMongoCollection<User> _users = database.GetCollection<User>(nameof(User));
        
        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _users.Find(u => u.Username == username).FirstOrDefaultAsync();
        }

        public async Task CreateUserAsync(User user)
        {
            await _users.InsertOneAsync(user);
        }

        public async Task<User?> GetUserByCredentialsAsync(string username, string password)
        {
            return await _users.Find(u => u.Username == username && u.Password == password).FirstOrDefaultAsync();
        }
    }
}

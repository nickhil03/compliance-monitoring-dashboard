namespace Compliance.Auth.ValidationLogic
{
    public static class Authentication
    {
        public static bool AuthenticateUser(string username, string password)
        {
            // Replace with your actual authentication logic
            return username == "testuser" && password == "password123";
        }
    }
}

const LoginPage = () => {
  return (
    <div>
      <h1>Login Page</h1>
      <form>
        <div>
          <label htmlFor="username">Username:</label>
          <input type="text" id="username" name="username" />
        </div>
        <div>
          <label htmlFor="password">Password:</label>
          <input type="password" id="password" name="password" />
        </div>
        <button type="submit">Login</button>
      </form>
    </div>
  );
};
export default LoginPage;
// This is a simple login page component in React. It includes a form with fields for username and password, and a submit button. The form does not have any functionality yet, but it can be extended to handle user authentication in the future.

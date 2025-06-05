import { useState } from "react";
import { useNavigate } from "react-router-dom"; // Import useNavigate for navigation

const LoginPage = ({ onLoginSuccess }) => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const navigate = useNavigate(); // Hook for navigation

  const handleLogin = async (e) => {
    try {
      e.preventDefault();
      setError("");
      if (username === "user" && password === "password") {
        console.log("Mock Login successful!");
        // In a real app, you'd get a token here and store it
        localStorage.setItem("authToken", "mock_jwt_token_123"); // Simulate storing a token
        onLoginSuccess(); // Call the function passed from App to update login state
        navigate("/dashboard"); // Navigate to dashboard on success
      } else {
        setError("Invalid username or password.");
      }
    } catch (error) {
      console.error("Error preventing default:", error);
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-100 font-sans antialiased">
      <div className="bg-white p-8 rounded-lg shadow-lg max-w-md w-full">
        <h2 className="text-3xl font-bold text-center text-gray-800 mb-6">
          Login to Dashboard
        </h2>
        <form onSubmit={handleLogin}>
          <div className="mb-4">
            <label
              htmlFor="username"
              className="block text-gray-700 text-sm font-bold mb-2"
            >
              Username
            </label>
            <input
              type="text"
              id="username"
              className="shadow appearance-none border rounded-md w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:ring-2 focus:ring-indigo-500"
              placeholder="Enter your username"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              required
            />
          </div>
          <div className="mb-6">
            <label
              htmlFor="password"
              className="block text-gray-700 text-sm font-bold mb-2"
            >
              Password
            </label>
            <input
              type="password"
              id="password"
              className="shadow appearance-none border rounded-md w-full py-2 px-3 text-gray-700 mb-3 leading-tight focus:outline-none focus:ring-2 focus:ring-indigo-500"
              placeholder="********"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
          </div>
          {error && (
            <p className="text-red-500 text-xs italic mb-4 text-center">
              {error}
            </p>
          )}
          <div className="flex items-center justify-between">
            <button
              type="submit"
              className="bg-indigo-600 hover:bg-indigo-700 text-white font-bold py-2 px-4 rounded-md focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2 w-full transition duration-150 ease-in-out"
            >
              Sign In
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default LoginPage;

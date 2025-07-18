import { Link } from "react-router-dom";

const Home = () => (
  <div className="flex items-center justify-center min-h-screen bg-gray-100">
    <div className="bg-white p-8 rounded shadow text-center">
      <h1 className="text-3xl font-bold mb-4">
        Welcome to Compliance Dashboard
      </h1>
      <p className="mb-6 text-gray-600">
        Please{" "}
        <Link to="/login" className="text-indigo-600 underline">
          login
        </Link>{" "}
        to continue.
      </p>
      <p className="mb-6 text-gray-600">
        <Link to="/signup" className="text-indigo-600 underline">
          Sign up
        </Link>{" "}
        for new account.
      </p>
    </div>
  </div>
);

export default Home;

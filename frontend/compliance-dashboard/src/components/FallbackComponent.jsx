const FallbackComponent = () => {
  return (
    <div className="min-h-screen flex items-center justify-center bg-red-100 font-sans antialiased text-center p-4">
      <div className="bg-white p-8 rounded-lg shadow-lg max-w-md w-full">
        <h2 className="text-3xl font-bold text-red-700 mb-4">
          Oops! Something went wrong.
        </h2>
        <p className="text-gray-700 mb-4">
          We're sorry, an unexpected error occurred within the application.
          Please try refreshing the page or contact support if the issue
          persists.
        </p>
      </div>
    </div>
  );
};

export default FallbackComponent;

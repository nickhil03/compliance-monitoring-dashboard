import axios from "axios";
import processQueue from "./processQueue.js";

let logoutHandler = null;

export const registerLogoutHandler = (handler) => {
  logoutHandler = handler;
};

const apiHandler = (url) => {
  // Create axios instance
  const api = axios.create({
    baseURL: url,
    withCredentials: true,
    headers: {
      "Content-Type": "application/json",
    },
  });

  axios.defaults.withCredentials = true;

  // Request interceptor - Add access token to requests
  api.interceptors.request.use(
    (config) => {
      const token = sessionStorage.getItem("accessToken");
      if (token) {
        config.headers.Authorization = `Bearer ${token}`;
      }
      return config;
    },
    (error) => {
      return Promise.reject(error);
    }
  );

  let isRefreshing = false;
  let failedQueue = [];

  const processQueueWrapper = (error, token = null) => {
    processQueue(failedQueue, error, token);
    failedQueue = [];
  };

  // Response interceptor - Handle token refresh
  api.interceptors.response.use(
    (response) => response,
    async (error) => {
      const originalRequest = error.config;

      if (
        error.response &&
        error.response?.status === 401 &&
        !originalRequest._retry
      ) {
        if (isRefreshing) {
          // If already refreshing, queue the request
          return new Promise((resolve, reject) => {
            failedQueue.push({ resolve, reject });
          })
            .then((token) => {
              originalRequest.headers.Authorization = `Bearer ${token}`;
              return api(originalRequest);
            })
            .catch((err) => {
              return Promise.reject(err);
            });
        }

        originalRequest._retry = true;
        isRefreshing = true;

        try {
          const response = await axios.post(
            `${url}auth/refresh`,
            {},
            {
              withCredentials: true,
            }
          );

          const { accessToken } = response.data;

          // Store new access token
          sessionStorage.setItem("accessToken", accessToken);

          // Process queued requests
          processQueueWrapper(null, accessToken);

          // Retry original request
          originalRequest.headers.Authorization = `Bearer ${accessToken}`;
          return api(originalRequest);
        } catch (refreshError) {
          processQueueWrapper(refreshError, null);

          // Refresh failed, redirect to login
          sessionStorage.removeItem("accessToken");

          // Call registered logout handler
          if (typeof logoutHandler === "function") {
            try {
              logoutHandler();
            } catch (e) {
              // swallow handler errors but fallback to hard redirect
              console.error("logoutHandler threw:", e);
              if (!window.location.pathname.includes("/login")) {
                window.location.href = "/login";
              }
            }
          } else {
            // Fallback for non-react usage (preserve previous behaviour)
            if (!window.location.pathname.includes("/login")) {
              window.location.href = "/login";
            }
          }

          return Promise.reject(refreshError);
        } finally {
          isRefreshing = false;
        }
      }

      return Promise.reject(error);
    }
  );

  return api;
};

export default apiHandler;

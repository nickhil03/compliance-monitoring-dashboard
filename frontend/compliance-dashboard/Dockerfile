# Step 1: Use official Node image
FROM node:20-alpine AS builder

# Step 2: Set working directory
WORKDIR /app

# Step 3: Copy package.json and install dependencies
COPY package*.json ./
RUN npm install

# Step 4: Copy all source files
COPY . .

# Make sure the environment is passed in properly
ARG VITE_AUTH_API_URL
ENV VITE_AUTH_API_URL=$VITE_AUTH_API_URL
ARG VITE_COMPLIANCE_API_URL
ENV VITE_COMPLIANCE_API_URL=$VITE_COMPLIANCE_API_URL

# Step 5: Build for production
RUN npm run build

# Step 6: Use Nginx to serve the built files
FROM nginx:alpine

# Step 7: Remove default Nginx page
RUN rm -rf /usr/share/nginx/html/*

# Step 8: Copy built files from the builder stage
COPY --from=builder /app/dist /usr/share/nginx/html

# Step 9: Copy custom Nginx configuration
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Step 10: Expose port for Nginx
EXPOSE 3000

# Step 11: Start Nginx
CMD ["nginx", "-g", "daemon off;"]
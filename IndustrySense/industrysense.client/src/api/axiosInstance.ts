// src/api/axiosInstance.ts
import axios from 'axios';

const axiosInstance = axios.create({
    baseURL: 'https://localhost:7210/api',
    timeout: 10000, // 请求超时时间
    headers: {
        'Content-Type': 'application/json',
    },
});

// 请求拦截器
axiosInstance.interceptors.request.use(
    (config) => {
        // 在请求发送之前做一些处理
        return config;
    },
    (error) => {
        // 请求错误处理
        return Promise.reject(error);
    }
);

// 响应拦截器
axiosInstance.interceptors.response.use(
    (response) => {
        // 对响应数据做一些处理
        return response.data;
    },
    (error) => {
        // 响应错误处理
        return Promise.reject(error);
    }
);

export default axiosInstance;

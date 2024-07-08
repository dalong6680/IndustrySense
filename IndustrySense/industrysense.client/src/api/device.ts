// src/api/device.ts
import axiosInstance from './axiosInstance';

export const getDevices = () => {
    return axiosInstance.get('/Device/getall');
};

// 其他设备相关的 API 调用函数

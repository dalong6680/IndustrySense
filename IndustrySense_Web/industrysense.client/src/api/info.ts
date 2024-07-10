import axiosInstance from './axiosInstance';

export const getOnlineDeviceCount = async () => {
    return axiosInstance.get('/Info/onlineDeviceCount');
};
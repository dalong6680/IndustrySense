import axiosInstance from './axiosInstance';

export const getDevices = () => {
    return axiosInstance.get('/Device/getall');
};

export const deleteDeviceById = (id: number) => {
    return axiosInstance.get(`/Device/remove/${id}`);
};

export const addDevice = (device: { DeviceId: number; DeviceName: string; DeviceIpAddress: string; ParsingRuleId: number }) => {
    return axiosInstance.get('/Device/add', {
        params: device
    });
};

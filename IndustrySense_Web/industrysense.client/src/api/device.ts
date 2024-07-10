import axiosInstance from './axiosInstance';

export const getDevices = () => {
    return axiosInstance.get('/Device/getall');
};

export const getDeviceCount = () => {
    return axiosInstance.get('/Device/getall')
        .then(response => response.resultCount);
};

export const deleteDeviceById = (id: number) => {
    return axiosInstance.get(`/Device/remove/${id}`);
};

export const addDevice = (device: { DeviceId: number; DeviceName: string; DeviceIpAddress: string; ParsingRuleId: number }) => {
    return axiosInstance.get('/Device/add', {
        params: device
    });
};

export const updateDevice = (id: number, device: { DeviceId: number; DeviceName: string; DeviceIpAddress: string; ParsingRuleId: number }) => {
    return axiosInstance.get('/Device/update', {
        params: {
            id,
            DeviceId: device.DeviceId,
            DeviceName: device.DeviceName,
            DeviceIpAddress: device.DeviceIpAddress,
            ParsingRuleId: device.ParsingRuleId
        }
    });
};

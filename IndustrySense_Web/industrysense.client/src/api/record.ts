import axiosInstance from './axiosInstance';

export const getRecords = async () => {
    return axiosInstance.get('/record/getall?pageIndex=-1')
}
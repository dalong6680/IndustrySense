import axiosInstance from './axiosInstance';

export const getRuleCount = () => {
    return axiosInstance.get('/ParsingRule/getall')
        .then(response => response.resultCount);
};

export const getParsingRules = () => {
    return axiosInstance.get('/ParsingRule/getall');
};

export const addParsingRule = (rule: { Name: string; Script: string }) => {
    return axiosInstance.get('/ParsingRule/add', {
        params: rule
    });
};

export const updateParsingRule = (id: number, rule: { Name: string; Script: string }) => {
    return axiosInstance.get('/ParsingRule/update', {
        params: { id, ...rule }
    });
};

export const deleteParsingRuleById = (id: number) => {
    return axiosInstance.get('/ParsingRule/delete', {
        params: { id }
    });
};
import axios from "axios";
import {FactionDto, OperatorClassDto, OperatorDetailsDto, OperatorDto, PagedResult, SearchRequest} from "../types";

const API_BASE_URL = 'http://localhost:5000/api';
const SERVER_BASE_URL = 'http://localhost:5000';

const api = axios.create({
    baseURL: API_BASE_URL,
    headers: {
        'Content-Type': 'application/json',
    },
});

const transformImageUrl = (imageUrl: string): string => {
    if (imageUrl.startsWith('http')) {
        return imageUrl; // Уже полный URL
    }
    return `${SERVER_BASE_URL}${imageUrl}`; // Добавляем базовый URL сервера
};

const processOperatorData = <T extends { imageUrl: string }>(data: T): T => {
    return {
        ...data,
        imageUrl: transformImageUrl(data.imageUrl)
    };
};

export const operatorApi = {
    getAll: async () => {
        const response = await api.get<OperatorDto[]>('/operators');
        return {
            ...response,
            data: response.data.map(processOperatorData)
        };
    },

    getById: async (id: number) => {
        const response = await api.get<OperatorDetailsDto>(`/operators/${id}`);
        return {
            ...response,
            data: processOperatorData(response.data)
        };
    },

    search: async (name: string) => {
        const response = await api.get<OperatorDto[]>(`/operators/search?name=${encodeURIComponent(name)}`);
        return {
            ...response,
            data: response.data.map(processOperatorData)
        };
    },

    getPaged: async (page: number, pageSize: number) => {
        const response = await api.get<PagedResult<OperatorDto>>(`/operators/paged?page=${page}&pageSize=${pageSize}`);
        return {
            ...response,
            data: {
                ...response.data,
                items: response.data.items.map(processOperatorData)
            }
        };
    },

    searchAdvanced: async (request: SearchRequest) => {
        const response = await api.post<PagedResult<OperatorDto>>('/operators/search/advanced', request);
        return {
            ...response,
            data: {
                ...response.data,
                items: response.data.items.map(processOperatorData)
            }
        };
    },

    getByRarity: async (rarity: number) => {
        const response = await api.get<OperatorDto[]>(`/operators/rarity/${rarity}`);
        return {
            ...response,
            data: response.data.map(processOperatorData)
        };
    },

    getNewest: async (count: number = 10) => {
        const response = await api.get<OperatorDto[]>(`/operators/newest?count=${count}`);
        return {
            ...response,
            data: response.data.map(processOperatorData)
        };
    },
};

export const referenceApi = {
    getClasses: () => api.get<OperatorClassDto[]>('/references/classes'),
    getFactions: () => api.get<FactionDto[]>('/references/factions'),
};

export default api;

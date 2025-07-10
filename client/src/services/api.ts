import axios from "axios";
import {FactionDto, OperatorClassDto, OperatorDetailsDto, OperatorDto, PagedResult, SearchRequest} from "../types";

const API_BASE_URL = 'http://localhost:5000/api'

const api = axios.create({
    baseURL: API_BASE_URL,
    headers: {
        'Content-Type': 'application/json',
    },
});

export const operatorApi = {
    getAll: () => api.get<OperatorDto[]>('/operators'),
    getById: (id: number) => api.get<OperatorDetailsDto>(`/operators/${id}`),
    
    search:(name:string) => api.get<OperatorDto[]>(`/operators/search?name=${encodeURIComponent(name)}`),
    getPaged: (page: number, pageSize: number) =>
        api.get<PagedResult<OperatorDto>>(`/operators/paged?page=${page}&pageSize=${pageSize}`),
    searchAdvanced: (request: SearchRequest) =>
        api.post<PagedResult<OperatorDto>>('/operators/search/advanced', request),
    
    getByRarity: (rarity: number) => api.get<OperatorDto[]>(`/operators/rarity/${rarity}`),
    getNewest: (count: number = 10) => api.get<OperatorDto[]>(`/operators/newest?count=${count}`),
}

export const referenceApi = {
    getClasses: () => api.get<OperatorClassDto[]>('/references/classes'),
    getFactions: () => api.get<FactionDto[]>('/references/factions'),
};

export default api;
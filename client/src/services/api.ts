import axios from "axios";
import {
    AuthResponse,
    FactionDto,
    LoginRequest,
    OperatorClassDto,
    OperatorDetailsDto,
    OperatorDto,
    PagedResult,
    RegisterRequest,
    SearchRequest,
    SubClassDto
} from "../types";

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
    
    getForEdit: async (id: number) => {
        return await api.get(`/operators/${id}/edit`);
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

    getSubClassesByClass: (classId: number) =>
        api.get<SubClassDto[]>(`/references/classes/${classId}/subclasses`),
};

export const authApi = {
    login: (credentials: LoginRequest) =>
        api.post<AuthResponse>('/auth/login', credentials),

    register: (data: RegisterRequest) =>
        api.post<AuthResponse>('/auth/register', data),
};

export const setAuthToken = (token: string | null) => {
    if (token) {
        api.defaults.headers.common['Authorization'] = `Bearer ${token}`;
    } else {
        delete api.defaults.headers.common['Authorization'];
    }
}

export const adminOperatorApi = {
    create: (operatorData: any) =>
        api.post('/operators', operatorData),

    update: (id: number, operatorData: any) =>
        api.put(`/operators/${id}`, operatorData),

    delete: (id: number) =>
        api.delete(`/operators/${id}`)
};

export const adminReferenceApi = {
    getUsers: () => api.get('/admin/users'),

    updateUserRole: (userId: number, role: string) =>
        api.put(`/admin/users/${userId}/role`, {role}),
};

// API для загрузки файлов
export const fileUploadApi = {
    uploadOperatorImage:(file:File,operatorName:string)=>{
        const formData = new FormData();
        formData.append('file', file);
        formData.append('operatorName', operatorName);
        
        return api.post('/fileupload/operator-image', formData, {
            headers: {
                'Content-Type': 'multipart/form-data'
            },
        });
    },
};

export default api;

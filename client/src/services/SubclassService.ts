import axios from 'axios';
import type { Subclass, CreateSubclass } from '../types/class';
import type { Operator } from '../types/operator';

export const subclassService = {
    // Получить все подклассы
    async getAll(): Promise<Subclass[]> {
        const response = await axios.get('/api/Subclasses');
        return response.data;
    },

    // Получить подкласс по ID
    async getById(id: number): Promise<Subclass> {
        const response = await axios.get(`/api/Subclasses/${id}`);
        return response.data;
    },

    // Создать новый подкласс
    async create(subclass: CreateSubclass): Promise<Subclass> {
        const response = await axios.post('/api/Subclasses', subclass);
        return response.data;
    },

    // Обновить подкласс
    async update(id: number, subclass: CreateSubclass): Promise<Subclass> {
        const response = await axios.put(`/api/Subclasses/${id}`, subclass);
        return response.data;
    },

    // Удалить подкласс
    async delete(id: number): Promise<void> {
        await axios.delete(`/api/Subclasses/${id}`);
    },

    // Получить операторов подкласса
    async getOperators(subclassId: number): Promise<Operator[]> {
        const response = await axios.get(`/api/Subclasses/${subclassId}/operators`);
        return response.data;
    }
};
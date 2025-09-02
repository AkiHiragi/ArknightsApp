import axios from 'axios';
import type { Class, Subclass } from '../types/class';
import type { Operator } from '../types/operator';

export const classService = {
    // Получить все классы
    async getAll(): Promise<Class[]> {
        const response = await axios.get('/api/Classes');
        return response.data;
    },

    // Получить класс по ID
    async getById(id: number): Promise<Class> {
        const response = await axios.get(`/api/Classes/${id}`);
        return response.data;
    },

    // Получить подклассы для класса
    async getSubclasses(classId: number): Promise<Subclass[]> {
        const response = await axios.get(`/api/Classes/${classId}/subclasses`);
        return response.data;
    },

    // Получить операторов класса
    async getOperators(classId: number): Promise<Operator[]> {
        const response = await axios.get(`/api/Classes/${classId}/operators`);
        return response.data;
    }
};
import axios from 'axios';
import type { Operator, OperatorDto } from "../types/operator.ts";

const api = axios.create({
    baseURL: '/api/operators'
});

export const operatorService = {
    async getAll(): Promise<Operator[]> {
        const { data } = await api.get<Operator[]>('');
        return data;
    },

    async getById(id: number): Promise<Operator> {
        const { data } = await api.get<Operator>(`/${id}`);
        return data;
    },

    async create(dto: OperatorDto): Promise<Operator> {
        const { data } = await api.post<Operator>('', dto);
        return data;
    },

    async update(id: number, dto: OperatorDto): Promise<Operator> {
        const { data } = await api.put<Operator>(`/${id}`, dto);
        return data;
    },

    async delete(id: number): Promise<void> {
        await api.delete(`/${id}`);
    }
};

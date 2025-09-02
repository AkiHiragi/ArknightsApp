export interface Class {
    id: number;
    name: string;
    description: string;
}

export interface Subclass {
    id: number;
    name: string;
    description: string;
    classId: number;
}

export interface CreateSubclass {
    name: string;
    description: string;
    classId: number;
}
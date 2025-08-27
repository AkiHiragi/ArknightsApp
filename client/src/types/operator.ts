export interface Operator {
    id: number;
    name: string;
    rarity: number;
    class: string;
    subclass: string;
    description: string;
    createdAt: string;
}

export interface OperatorDto {
    name: string;
    rarity: number;
    class: string;
    subclass: string;
    description: string;
}
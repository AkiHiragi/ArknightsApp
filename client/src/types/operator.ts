export interface Operator {
    id: number;
    name: string;
    rarity: number;
    class: string;
    subclass: string;
    description: string;
    createdAt: string;
    
    avatarUrl?:string;
    previewUrl?:string;
    e0ArtUrl?:string;
    e2ArtUrl?:string;
}

export interface OperatorDto {
    name: string;
    rarity: number;
    class: string;
    subclass: string;
    description: string;

    avatarUrl?:string;
    previewUrl?:string;
    e0ArtUrl?:string;
    e2ArtUrl?:string;
}
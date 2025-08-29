export interface Operator {
    id: number;
    name: string;
    rarity: number;
    classId:number;
    className: string;
    subclassId:number;
    subclassName: string;
    description: string;
    
    avatarUrl?:string;
    previewUrl?:string;
    e0ArtUrl?:string;
    e2ArtUrl?:string;
}
import exp from "node:constants";

export interface OperatorDto {
    id: number;
    name: string;
    rarity: number;
    className: string;
    subClassName: string;
    factionName?: string;
    imageUrl: string;
    description: string;
    position: string;
    cnReleaseDate: string;
    globalReleaseDate?: string;
    isGlobalReleased: boolean;
    releaseStatus: string;
}

export interface OperatorDetailsDto extends OperatorDto {
    skills: SkillDto[];
    talents: TalentDto[];
    baseStats?: OperatorBaseStatsDto;
}

export interface SkillDto {
    id: number;
    name: string;
    description: string;
    iconUrl: string;
    levels: string;
}

export interface SkillLevelDto {
    level: number;
    levelName: string;
    spCost: number;
    duration: number;
    description: string;
}

export interface TalentDto {
    id: number;
    name: string;
    description: string;
    iconUrl: string;
    levels: TalentLevelDto[];
}

export interface TalentLevelDto {
    potentialLevel: number;
    unlockCondition: string;
    description: string;
}

export interface OperatorBaseStatsDto {
    attackSpeed: number;
    attackType: string;
    blockCount: number;
    deploymentCost: number;
    redeployTime: number;
    maxE0Level: number;
    maxE1Level?: number;
    maxE2Level?: number;
}

// Типы для поиска и пагинации
export interface PagedResult<T> {
    items: T[];
    totalCount: number;
    page: number;
    pageSize: number;
    totalPages: number;
    hasNextPage: boolean;
    hasPreviousPage: boolean;
}

export interface SearchRequest {
    name?: string;
    rarity?: number;
    className?: string;
    factionName?: string;
    page: number;
    pageSize: number;
    sortBy: string;
    sortDescending: boolean;
}

// Справочные типы
export interface OperatorClassDto {
    id: number;
    name: string;
    description: string;
    iconUrl?: string;
}

export interface FactionDto {
    id: number;
    name: string;
    description: string;
    iconUrl?: string;
    logoUrl?: string;
}

// Типы для аутентификации
export interface LoginRequest {
    username: string;
    password: string;
}

export interface RegisterRequest {
    username: string;
    email: string;
    password: string;
    confirmPassword: string;
}

export interface AuthResponse {
    token: string;
    user: User;
    expiresAt: string;
}

export interface User {
    id: number;
    username: string;
    email: string;
    role: string;
    createdAt: string;
    lastLoginAt?: string;
}

// Контекст аутентификации
export interface AuthContextType {
    user: User | null;
    token: string | null;
    login: (credentials:LoginRequest) => Promise<void>;
    register: (data: RegisterRequest) => Promise<void>;
    logout: () => void;
    isAuthenticated: boolean;
    isAdmin: boolean;
    isModerator: boolean;
}
import React, { createContext, useContext, useState, useEffect, ReactNode } from 'react';
import { AuthContextType, User, LoginRequest, RegisterRequest } from '../types';
import { authApi, setAuthToken } from '../services/api';

const AuthContext = createContext<AuthContextType | undefined>(undefined);

interface AuthProviderProps {
    children: ReactNode;
}

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
    const [user, setUser] = useState<User | null>(null);
    const [token, setToken] = useState<string | null>(null);
    const [loading, setLoading] = useState(true);

    // Загружаем данные из localStorage при инициализации
    useEffect(() => {
        const savedToken = localStorage.getItem('auth_token');
        const savedUser = localStorage.getItem('auth_user');

        if (savedToken && savedUser) {
            try {
                const parsedUser = JSON.parse(savedUser);
                setToken(savedToken);
                setUser(parsedUser);
                setAuthToken(savedToken);
            } catch (error) {
                console.error('Error parsing saved user data:', error);
                localStorage.removeItem('auth_token');
                localStorage.removeItem('auth_user');
            }
        }
        setLoading(false);
    }, []);

    const login = async (credentials: LoginRequest) => {
        try {
            const response = await authApi.login(credentials);
            const { token: newToken, user: newUser } = response.data;

            setToken(newToken);
            setUser(newUser);
            setAuthToken(newToken);

            // Сохраняем в localStorage
            localStorage.setItem('auth_token', newToken);
            localStorage.setItem('auth_user', JSON.stringify(newUser));
        } catch (error) {
            console.error('Login error:', error);
            throw error;
        }
    };

    const register = async (data: RegisterRequest) => {
        try {
            const response = await authApi.register(data);
            const { token: newToken, user: newUser } = response.data;

            setToken(newToken);
            setUser(newUser);
            setAuthToken(newToken);

            // Сохраняем в localStorage
            localStorage.setItem('auth_token', newToken);
            localStorage.setItem('auth_user', JSON.stringify(newUser));
        } catch (error) {
            console.error('Register error:', error);
            throw error;
        }
    };

    const logout = () => {
        setToken(null);
        setUser(null);
        setAuthToken(null);

        // Удаляем из localStorage
        localStorage.removeItem('auth_token');
        localStorage.removeItem('auth_user');
    };

    const value: AuthContextType = {
        user,
        token,
        login,
        register,
        logout,
        isAuthenticated: !!user,
        isAdmin: user?.role === 'Admin',
        isModerator: user?.role === 'Moderator' || user?.role === 'Admin',
    };

    if (loading) {
        return <div className="d-flex justify-content-center align-items-center" style={{ height: '100vh' }}>
            <div className="spinner-border" role="status">
                <span className="visually-hidden">Загрузка...</span>
            </div>
        </div>;
    }

    return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

export const useAuth = (): AuthContextType => {
    const context = useContext(AuthContext);
    if (context === undefined) {
        throw new Error('useAuth must be used within an AuthProvider');
    }
    return context;
};

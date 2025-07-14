import React from "react";
import {useAuth} from "../contexts/AuthContext";
import {Alert, Button} from "react-bootstrap";

interface ProtectedRouteProps {
    children: React.ReactNode;
    requiredRole?: 'User' | 'Moderator' | 'Admin';
}

const ProtectedRoute: React.FC<ProtectedRouteProps> = ({
                                                           children,
                                                           requiredRole = 'User'
                                                       }) => {
    const {isAuthenticated, user, isAdmin, isModerator} = useAuth();

    if (!isAuthenticated) {
        return (
            <Alert variant="warning" className="text-center">
                <Alert.Heading>Требуется авторизация</Alert.Heading>
                <p> Для доступа к этой странице необходимо войти в систему </p>
                <Button variant="primary" onClick={() => window.location.reload()}>
                    Обновить страницу
                </Button>
            </Alert>
        );
    }

    const hasAccess = () => {
        switch (requiredRole) {
            case 'Admin':
                return isAdmin;
            case 'Moderator':
                return isModerator;
            case 'User':
            default:
                return true;
        }
    }

    if (!hasAccess()) {
        return (
            <Alert variant="danger" className="text-center">
                <Alert.Heading>Доступ запрещен</Alert.Heading>
                <p>
                    У вас недостаточно прав для доступа к этой странице.
                    <br/>
                    <small className="text-muted">
                        Требуется роль: <strong>{requiredRole}</strong>, ваша роль: <strong>{user?.role}</strong>
                    </small>
                </p>
            </Alert>
        );
    }

    return <>{children}</>
};

export default ProtectedRoute;
import React from 'react';
import { Alert } from 'react-bootstrap';

const UserManagement: React.FC = () => {
    return (
        <div>
            <h2 className="mb-4">Управление пользователями</h2>
            <Alert variant="info">
                <Alert.Heading>В разработке</Alert.Heading>
                <p>Функционал управления пользователями будет добавлен в следующих версиях.</p>
                <hr />
                <p className="mb-0">
                    Планируется: просмотр пользователей, изменение ролей, блокировка аккаунтов.
                </p>
            </Alert>
        </div>
    );
};

export default UserManagement;

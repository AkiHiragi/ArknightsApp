import React from 'react';
import { Routes, Route, Navigate } from 'react-router-dom';
import { Row, Col, Card, Nav } from 'react-bootstrap';
import { Link, useLocation } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import AdminDashboard from '../components/admin/AdminDashboard';
import OperatorManagement from '../components/admin/OperatorManagement';
import UserManagement from '../components/admin/UserManagement';

const AdminPanel: React.FC = () => {
    const location = useLocation();
    const { user } = useAuth();

    const isActive = (path: string) => {
        return location.pathname === `/admin${path}` ||
            (path === '' && location.pathname === '/admin');
    };

    return (
        <div>
            <div className="d-flex justify-content-between align-items-center mb-4">
                <h1>
                    <i className="bi bi-gear me-2"></i>
                    Админ-панель
                </h1>
                <div className="text-muted">
                    Добро пожаловать, <strong>{user?.username}</strong>
                </div>
            </div>

            <Row>
                {/* Боковое меню */}
                <Col md={3}>
                    <Card>
                        <Card.Header>
                            <h6 className="mb-0">Навигация</h6>
                        </Card.Header>
                        <Card.Body className="p-0">
                            <Nav variant="pills" className="flex-column">
                                <Nav.Item>
                                    <Nav.Link
                                        as={Link}
                                        to="/admin"
                                        active={isActive('')}
                                        className="rounded-0"
                                    >
                                        <i className="bi bi-speedometer2 me-2"></i>
                                        Dashboard
                                    </Nav.Link>
                                </Nav.Item>
                                <Nav.Item>
                                    <Nav.Link
                                        as={Link}
                                        to="/admin/operators"
                                        active={isActive('/operators')}
                                        className="rounded-0"
                                    >
                                        <i className="bi bi-people me-2"></i>
                                        Управление операторами
                                    </Nav.Link>
                                </Nav.Item>
                                <Nav.Item>
                                    <Nav.Link
                                        as={Link}
                                        to="/admin/users"
                                        active={isActive('/users')}
                                        className="rounded-0"
                                    >
                                        <i className="bi bi-person-gear me-2"></i>
                                        Управление пользователями
                                    </Nav.Link>
                                </Nav.Item>
                            </Nav>
                        </Card.Body>
                    </Card>
                </Col>

                {/* Основной контент */}
                <Col md={9}>
                    <Routes>
                        <Route path="/" element={<AdminDashboard />} />
                        <Route path="/operators/*" element={<OperatorManagement />} />
                        <Route path="/users" element={<UserManagement />} />
                        <Route path="*" element={<Navigate to="/admin" replace />} />
                    </Routes>
                </Col>
            </Row>
        </div>
    );
};

export default AdminPanel;

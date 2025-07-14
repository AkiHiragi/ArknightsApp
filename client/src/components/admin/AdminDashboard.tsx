import React, { useEffect, useState } from 'react';
import { Row, Col, Card, Alert, Spinner } from 'react-bootstrap';
import { operatorApi, referenceApi } from '../../services/api';

interface DashboardStats {
    totalOperators: number;
    totalClasses: number;
    totalFactions: number;
    recentOperators: number;
}

const AdminDashboard: React.FC = () => {
    const [stats, setStats] = useState<DashboardStats | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchStats = async () => {
            try {
                const [operatorsRes, classesRes, factionsRes] = await Promise.all([
                    operatorApi.getAll(),
                    referenceApi.getClasses(),
                    referenceApi.getFactions()
                ]);

                const operators = operatorsRes.data;
                const recentDate = new Date();
                recentDate.setMonth(recentDate.getMonth() - 6); // Последние 6 месяцев

                const recentOperators = operators.filter(op =>
                    new Date(op.cnReleaseDate) > recentDate
                ).length;

                setStats({
                    totalOperators: operators.length,
                    totalClasses: classesRes.data.length,
                    totalFactions: factionsRes.data.length,
                    recentOperators
                });
            } catch (err) {
                setError('Ошибка загрузки статистики');
                console.error('Dashboard stats error:', err);
            } finally {
                setLoading(false);
            }
        };

        fetchStats();
    }, []);

    if (loading) {
        return (
            <div className="text-center">
                <Spinner animation="border" role="status">
                    <span className="visually-hidden">Загрузка...</span>
                </Spinner>
            </div>
        );
    }

    if (error) {
        return <Alert variant="danger">{error}</Alert>;
    }

    return (
        <div>
            <h2 className="mb-4">Dashboard</h2>

            {/* Статистические карточки */}
            <Row className="mb-4">
                <Col md={3}>
                    <Card className="text-center bg-primary text-white">
                        <Card.Body>
                            <div className="display-6">
                                <i className="bi bi-people"></i>
                            </div>
                            <h3>{stats?.totalOperators}</h3>
                            <p className="mb-0">Всего операторов</p>
                        </Card.Body>
                    </Card>
                </Col>
                <Col md={3}>
                    <Card className="text-center bg-success text-white">
                        <Card.Body>
                            <div className="display-6">
                                <i className="bi bi-grid"></i>
                            </div>
                            <h3>{stats?.totalClasses}</h3>
                            <p className="mb-0">Классов</p>
                        </Card.Body>
                    </Card>
                </Col>
                <Col md={3}>
                    <Card className="text-center bg-info text-white">
                        <Card.Body>
                            <div className="display-6">
                                <i className="bi bi-flag"></i>
                            </div>
                            <h3>{stats?.totalFactions}</h3>
                            <p className="mb-0">Фракций</p>
                        </Card.Body>
                    </Card>
                </Col>
                <Col md={3}>
                    <Card className="text-center bg-warning text-white">
                        <Card.Body>
                            <div className="display-6">
                                <i className="bi bi-clock"></i>
                            </div>
                            <h3>{stats?.recentOperators}</h3>
                            <p className="mb-0">Новых за 6 мес.</p>
                        </Card.Body>
                    </Card>
                </Col>
            </Row>

            {/* Быстрые действия */}
            <Row>
                <Col md={6}>
                    <Card>
                        <Card.Header>
                            <h5 className="mb-0">Быстрые действия</h5>
                        </Card.Header>
                        <Card.Body>
                            <div className="d-grid gap-2">
                                <a href="/admin/operators/create" className="btn btn-primary">
                                    <i className="bi bi-plus-circle me-2"></i>
                                    Добавить оператора
                                </a>
                                <a href="/admin/operators" className="btn btn-outline-primary">
                                    <i className="bi bi-list me-2"></i>
                                    Управление операторами
                                </a>
                                <a href="/admin/users" className="btn btn-outline-secondary">
                                    <i className="bi bi-person-gear me-2"></i>
                                    Управление пользователями
                                </a>
                            </div>
                        </Card.Body>
                    </Card>
                </Col>
                <Col md={6}>
                    <Card>
                        <Card.Header>
                            <h5 className="mb-0">Системная информация</h5>
                        </Card.Header>
                        <Card.Body>
                            <ul className="list-unstyled mb-0">
                                <li className="mb-2">
                                    <strong>Версия API:</strong> v1.0
                                </li>
                                <li className="mb-2">
                                    <strong>База данных:</strong> SQLite
                                </li>
                                <li className="mb-2">
                                    <strong>Последнее обновление:</strong> {new Date().toLocaleDateString('ru-RU')}
                                </li>
                                <li>
                                    <strong>Статус:</strong>
                                    <span className="badge bg-success ms-2">Активен</span>
                                </li>
                            </ul>
                        </Card.Body>
                    </Card>
                </Col>
            </Row>
        </div>
    );
};

export default AdminDashboard;

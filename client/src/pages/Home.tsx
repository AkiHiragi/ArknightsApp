import React, { useEffect, useState } from 'react';
import { Row, Col, Card, Button, Alert } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import { operatorApi } from '../services/api';
import { OperatorDto } from '../types';
import OperatorCard from '../components/OperatorCard';

const Home: React.FC = () => {
    const [newestOperators, setNewestOperators] = useState<OperatorDto[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchNewestOperators = async () => {
            try {
                const response = await operatorApi.getNewest(6);
                setNewestOperators(response.data);
            } catch (err) {
                setError('Ошибка загрузки данных');
                console.error('Error fetching newest operators:', err);
            } finally {
                setLoading(false);
            }
        };

        fetchNewestOperators();
    }, []);

    if (loading) {
        return <div className="text-center">Загрузка...</div>;
    }

    return (
        <div>
            {/* Hero секция */}
            <Card className="mb-4 bg-primary text-white">
                <Card.Body className="text-center py-5">
                    <h1 className="display-4 mb-3">
                        <i className="bi bi-shield-check me-3"></i>
                        Arknights Operators Database
                    </h1>
                    <p className="lead mb-4">
                        Полная база данных операторов из игры Arknights с характеристиками, навыками и талантами
                    </p>
                    <Link to="/operators" className="btn btn-light btn-lg text-decoration-none">
                        Просмотреть всех операторов
                    </Link>
                </Card.Body>
            </Card>

            {/* Статистика */}
            <Row className="mb-4">
                <Col md={3}>
                    <Card className="text-center">
                        <Card.Body>
                            <h3 className="text-primary">5</h3>
                            <p className="mb-0">Операторов в базе</p>
                        </Card.Body>
                    </Card>
                </Col>
                <Col md={3}>
                    <Card className="text-center">
                        <Card.Body>
                            <h3 className="text-success">8</h3>
                            <p className="mb-0">Классов</p>
                        </Card.Body>
                    </Card>
                </Col>
                <Col md={3}>
                    <Card className="text-center">
                        <Card.Body>
                            <h3 className="text-warning">5</h3>
                            <p className="mb-0">Фракций</p>
                        </Card.Body>
                    </Card>
                </Col>
                <Col md={3}>
                    <Card className="text-center">
                        <Card.Body>
                            <h3 className="text-info">3</h3>
                            <p className="mb-0">6★ операторов</p>
                        </Card.Body>
                    </Card>
                </Col>
            </Row>

            {/* Новейшие операторы */}
            <h2 className="mb-3">Новейшие операторы</h2>
            {error ? (
                <Alert variant="danger">{error}</Alert>
            ) : (
                <Row>
                    {newestOperators.map(operator => (
                        <Col key={operator.id} md={4} className="mb-3">
                            <OperatorCard operator={operator} />
                        </Col>
                    ))}
                </Row>
            )}
        </div>
    );
};

export default Home;

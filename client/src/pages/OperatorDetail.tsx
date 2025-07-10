import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { Row, Col, Card, Badge, Button, Alert, Spinner, ListGroup } from 'react-bootstrap';
import { operatorApi } from '../services/api';
import { OperatorDetailsDto } from '../types';

const OperatorDetail: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const [operator, setOperator] = useState<OperatorDetailsDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchOperator = async () => {
            if (!id) return;

            try {
                setLoading(true);
                const response = await operatorApi.getById(Number(id));
                setOperator(response.data);
            } catch (err) {
                setError('Оператор не найден');
                console.error('Error fetching operator:', err);
            } finally {
                setLoading(false);
            }
        };

        fetchOperator();
    }, [id]);

    const getRarityStars = (rarity: number) => {
        return '★'.repeat(rarity);
    };

    const getRarityColor = (rarity: number) => {
        const colors = {
            1: 'secondary',
            2: 'success',
            3: 'info',
            4: 'primary',
            5: 'warning',
            6: 'danger'
        };
        return colors[rarity as keyof typeof colors] || 'secondary';
    };

    if (loading) {
        return (
            <div className="text-center">
                <Spinner animation="border" role="status">
                    <span className="visually-hidden">Загрузка...</span>
                </Spinner>
            </div>
        );
    }

    if (error || !operator) {
        return (
            <div>
                <Alert variant="danger">{error || 'Оператор не найден'}</Alert>
                <Button variant="primary" onClick={() => navigate('/operators')}>
                    Вернуться к списку
                </Button>
            </div>
        );
    }

    return (
        <div>
            {/* Кнопка назад */}
            <Button
                variant="outline-primary"
                className="mb-3"
                onClick={() => navigate('/operators')}
            >
                <i className="bi bi-arrow-left me-2"></i>
                Назад к списку
            </Button>

            <Row>
                {/* Левая колонка - изображение и основная информация */}
                <Col lg={4}>
                    <Card className="mb-4">
                        <Card.Img
                            variant="top"
                            src={operator.imageUrl}
                            alt={operator.name}
                            style={{ height: '400px', objectFit: 'cover' }}
                            onError={(e) => {
                                (e.target as HTMLImageElement).src = 'https://via.placeholder.com/400x400?text=No+Image';
                            }}
                        />
                        <Card.Body>
                            <div className="d-flex justify-content-between align-items-center mb-3">
                                <Card.Title className="mb-0 h3">{operator.name}</Card.Title>
                                <Badge bg={getRarityColor(operator.rarity)} className="fs-6">
                                    {getRarityStars(operator.rarity)}
                                </Badge>
                            </div>

                            <ListGroup variant="flush">
                                <ListGroup.Item className="px-0">
                                    <strong>Класс:</strong> {operator.className}
                                </ListGroup.Item>
                                <ListGroup.Item className="px-0">
                                    <strong>Подкласс:</strong> {operator.subClassName}
                                </ListGroup.Item>
                                {operator.factionName && (
                                    <ListGroup.Item className="px-0">
                                        <strong>Фракция:</strong> {operator.factionName}
                                    </ListGroup.Item>
                                )}
                                <ListGroup.Item className="px-0">
                                    <strong>Позиция:</strong> {operator.position}
                                </ListGroup.Item>
                                <ListGroup.Item className="px-0">
                                    <strong>Релиз CN:</strong> {new Date(operator.cnReleaseDate).toLocaleDateString('ru-RU')}
                                </ListGroup.Item>
                                {operator.globalReleaseDate && (
                                    <ListGroup.Item className="px-0">
                                        <strong>Релиз Global:</strong> {new Date(operator.globalReleaseDate).toLocaleDateString('ru-RU')}
                                    </ListGroup.Item>
                                )}
                                <ListGroup.Item className="px-0">
                                    <strong>Статус:</strong>
                                    <Badge
                                        bg={operator.isGlobalReleased ? 'success' : 'warning'}
                                        className="ms-2"
                                    >
                                        {operator.releaseStatus}
                                    </Badge>
                                </ListGroup.Item>
                            </ListGroup>
                        </Card.Body>
                    </Card>
                </Col>

                {/* Правая колонка - описание, навыки, таланты */}
                <Col lg={8}>
                    {/* Описание */}
                    <Card className="mb-4">
                        <Card.Header>
                            <h5 className="mb-0">Описание</h5>
                        </Card.Header>
                        <Card.Body>
                            <p>{operator.description}</p>
                        </Card.Body>
                    </Card>

                    {/* Базовые характеристики */}
                    {operator.baseStats && (
                        <Card className="mb-4">
                            <Card.Header>
                                <h5 className="mb-0">Базовые характеристики</h5>
                            </Card.Header>
                            <Card.Body>
                                <Row>
                                    <Col md={6}>
                                        <ListGroup variant="flush">
                                            <ListGroup.Item className="px-0">
                                                <strong>Скорость атаки:</strong> {operator.baseStats.attackSpeed}с
                                            </ListGroup.Item>
                                            <ListGroup.Item className="px-0">
                                                <strong>Тип атаки:</strong> {operator.baseStats.attackType}
                                            </ListGroup.Item>
                                            <ListGroup.Item className="px-0">
                                                <strong>Блок:</strong> {operator.baseStats.blockCount}
                                            </ListGroup.Item>
                                        </ListGroup>
                                    </Col>
                                    <Col md={6}>
                                        <ListGroup variant="flush">
                                            <ListGroup.Item className="px-0">
                                                <strong>Стоимость:</strong> {operator.baseStats.deploymentCost} DP
                                            </ListGroup.Item>
                                            <ListGroup.Item className="px-0">
                                                <strong>Перезарядка:</strong> {operator.baseStats.redeployTime}с
                                            </ListGroup.Item>
                                            <ListGroup.Item className="px-0">
                                                <strong>Макс. уровни:</strong>
                                                <div>
                                                    E0: {operator.baseStats.maxE0Level}
                                                    {operator.baseStats.maxE1Level && ` • E1: ${operator.baseStats.maxE1Level}`}
                                                    {operator.baseStats.maxE2Level && ` • E2: ${operator.baseStats.maxE2Level}`}
                                                </div>
                                            </ListGroup.Item>
                                        </ListGroup>
                                    </Col>
                                </Row>
                            </Card.Body>
                        </Card>
                    )}

                    {/* Навыки */}
                    {operator.skills && operator.skills.length > 0 && (
                        <Card className="mb-4">
                            <Card.Header>
                                <h5 className="mb-0">Навыки</h5>
                            </Card.Header>
                            <Card.Body>
                                {operator.skills.map((skill, index) => (
                                    <div key={skill.id} className={index > 0 ? 'mt-3 pt-3 border-top' : ''}>
                                        <h6>{skill.name}</h6>
                                        <p className="text-muted">{skill.description}</p>
                                    </div>
                                ))}
                            </Card.Body>
                        </Card>
                    )}

                    {/* Таланты */}
                    {operator.talents && operator.talents.length > 0 && (
                        <Card className="mb-4">
                            <Card.Header>
                                <h5 className="mb-0">Таланты</h5>
                            </Card.Header>
                            <Card.Body>
                                {operator.talents.map((talent, index) => (
                                    <div key={talent.id} className={index > 0 ? 'mt-3 pt-3 border-top' : ''}>
                                        <h6>{talent.name}</h6>
                                        <p className="text-muted">{talent.description}</p>
                                    </div>
                                ))}
                            </Card.Body>
                        </Card>
                    )}
                </Col>
            </Row>
        </div>
    );
};

export default OperatorDetail;

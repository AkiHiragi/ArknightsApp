import React, { useEffect, useState } from 'react';
import { Row, Col, Form, Button, Alert, Spinner, Card } from 'react-bootstrap';
import { operatorApi, referenceApi } from '../services/api';
import { OperatorDto, OperatorClassDto, FactionDto, SearchRequest } from '../types';
import OperatorCard from '../components/OperatorCard';

const OperatorList: React.FC = () => {
    const [operators, setOperators] = useState<OperatorDto[]>([]);
    const [classes, setClasses] = useState<OperatorClassDto[]>([]);
    const [factions, setFactions] = useState<FactionDto[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    // Фильтры
    const [searchName, setSearchName] = useState('');
    const [selectedRarity, setSelectedRarity] = useState<number | undefined>();
    const [selectedClass, setSelectedClass] = useState('');
    const [selectedFaction, setSelectedFaction] = useState('');
    const [sortBy, setSortBy] = useState('name');
    const [sortDesc, setSortDesc] = useState(false);

    // Загрузка данных при монтировании
    useEffect(() => {
        const fetchData = async () => {
            try {
                setLoading(true);
                const [operatorsRes, classesRes, factionsRes] = await Promise.all([
                    operatorApi.getAll(),
                    referenceApi.getClasses(),
                    referenceApi.getFactions()
                ]);

                setOperators(operatorsRes.data);
                setClasses(classesRes.data);
                setFactions(factionsRes.data);
            } catch (err) {
                setError('Ошибка загрузки данных');
                console.error('Error fetching data:', err);
            } finally {
                setLoading(false);
            }
        };

        fetchData();
    }, []);

    // Поиск с фильтрами
    const handleSearch = async () => {
        try {
            setLoading(true);
            setError(null);

            const searchRequest: SearchRequest = {
                name: searchName || undefined,
                rarity: selectedRarity,
                className: selectedClass || undefined,
                factionName: selectedFaction || undefined,
                page: 1,
                pageSize: 50,
                sortBy: sortBy,
                sortDescending: sortDesc
            };

            const response = await operatorApi.searchAdvanced(searchRequest);
            setOperators(response.data.items);
        } catch (err) {
            setError('Ошибка поиска');
            console.error('Error searching:', err);
        } finally {
            setLoading(false);
        }
    };

    // Сброс фильтров
    const handleReset = async () => {
        setSearchName('');
        setSelectedRarity(undefined);
        setSelectedClass('');
        setSelectedFaction('');
        setSortBy('name');
        setSortDesc(false);

        try {
            setLoading(true);
            const response = await operatorApi.getAll();
            setOperators(response.data);
        } catch (err) {
            setError('Ошибка загрузки данных');
        } finally {
            setLoading(false);
        }
    };

    if (loading && operators.length === 0) {
        return (
            <div className="text-center">
                <Spinner animation="border" role="status">
                    <span className="visually-hidden">Загрузка...</span>
                </Spinner>
            </div>
        );
    }

    return (
        <div>
            <h1 className="mb-4">Операторы Arknights</h1>

            {/* Фильтры */}
            <Card className="mb-4">
                <Card.Body>
                    <Row>
                        <Col md={3}>
                            <Form.Group className="mb-3">
                                <Form.Label>Поиск по имени</Form.Label>
                                <Form.Control
                                    type="text"
                                    placeholder="Введите имя..."
                                    value={searchName}
                                    onChange={(e) => setSearchName(e.target.value)}
                                />
                            </Form.Group>
                        </Col>
                        <Col md={2}>
                            <Form.Group className="mb-3">
                                <Form.Label>Редкость</Form.Label>
                                <Form.Select
                                    value={selectedRarity || ''}
                                    onChange={(e) => setSelectedRarity(e.target.value ? Number(e.target.value) : undefined)}
                                >
                                    <option value="">Все</option>
                                    {[1, 2, 3, 4, 5, 6].map(rarity => (
                                        <option key={rarity} value={rarity}>
                                            {'★'.repeat(rarity)}
                                        </option>
                                    ))}
                                </Form.Select>
                            </Form.Group>
                        </Col>
                        <Col md={2}>
                            <Form.Group className="mb-3">
                                <Form.Label>Класс</Form.Label>
                                <Form.Select
                                    value={selectedClass}
                                    onChange={(e) => setSelectedClass(e.target.value)}
                                >
                                    <option value="">Все классы</option>
                                    {classes.map(cls => (
                                        <option key={cls.id} value={cls.name}>
                                            {cls.name}
                                        </option>
                                    ))}
                                </Form.Select>
                            </Form.Group>
                        </Col>
                        <Col md={2}>
                            <Form.Group className="mb-3">
                                <Form.Label>Фракция</Form.Label>
                                <Form.Select
                                    value={selectedFaction}
                                    onChange={(e) => setSelectedFaction(e.target.value)}
                                >
                                    <option value="">Все фракции</option>
                                    {factions.map(faction => (
                                        <option key={faction.id} value={faction.name}>
                                            {faction.name}
                                        </option>
                                    ))}
                                </Form.Select>
                            </Form.Group>
                        </Col>
                        <Col md={2}>
                            <Form.Group className="mb-3">
                                <Form.Label>Сортировка</Form.Label>
                                <Form.Select
                                    value={`${sortBy}-${sortDesc}`}
                                    onChange={(e) => {
                                        const [field, desc] = e.target.value.split('-');
                                        setSortBy(field);
                                        setSortDesc(desc === 'true');
                                    }}
                                >
                                    <option value="name-false">Имя ↑</option>
                                    <option value="name-true">Имя ↓</option>
                                    <option value="rarity-false">Редкость ↑</option>
                                    <option value="rarity-true">Редкость ↓</option>
                                    <option value="cnReleaseDate-false">Дата ↑</option>
                                    <option value="cnReleaseDate-true">Дата ↓</option>
                                </Form.Select>
                            </Form.Group>
                        </Col>
                        <Col md={1}>
                            <Form.Label>&nbsp;</Form.Label>
                            <div className="d-grid gap-2">
                                <Button variant="primary" onClick={handleSearch} disabled={loading}>
                                    {loading ? <Spinner size="sm" /> : 'Поиск'}
                                </Button>
                                <Button variant="outline-secondary" onClick={handleReset}>
                                    Сброс
                                </Button>
                            </div>
                        </Col>
                    </Row>
                </Card.Body>
            </Card>

            {/* Результаты */}
            {error && <Alert variant="danger">{error}</Alert>}

            <div className="mb-3">
                <small className="text-muted">
                    Найдено операторов: {operators.length}
                </small>
            </div>

            <Row>
                {operators.map(operator => (
                    <Col key={operator.id} lg={4} md={6} className="mb-4">
                        <OperatorCard operator={operator} />
                    </Col>
                ))}
            </Row>

            {operators.length === 0 && !loading && (
                <div className="text-center py-5">
                    <h4>Операторы не найдены</h4>
                    <p className="text-muted">Попробуйте изменить параметры поиска</p>
                </div>
            )}
        </div>
    );
};

export default OperatorList;

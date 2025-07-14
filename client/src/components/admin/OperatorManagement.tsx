import React, { useState, useEffect } from 'react';
import { Routes, Route, useNavigate, Link } from 'react-router-dom';
import { Table, Button, Alert, Spinner, Badge, Modal } from 'react-bootstrap';
import { operatorApi, adminOperatorApi } from '../../services/api';
import { OperatorDto } from '../../types';
import OperatorForm from './OperatorForm';

const OperatorManagement: React.FC = () => {
    return (
        <Routes>
            <Route path="/" element={<OperatorList />} />
            <Route path="/create" element={<CreateOperator />} />
            <Route path="/edit/:id" element={<EditOperator />} />
        </Routes>
    );
};

const OperatorList: React.FC = () => {
    const [operators, setOperators] = useState<OperatorDto[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [deleteModal, setDeleteModal] = useState<{ show: boolean; operator: OperatorDto | null }>({
        show: false,
        operator: null
    });
    const navigate = useNavigate();

    useEffect(() => {
        fetchOperators();
    }, []);

    const fetchOperators = async () => {
        try {
            const response = await operatorApi.getAll();
            setOperators(response.data);
        } catch (err) {
            setError('Ошибка загрузки операторов');
        } finally {
            setLoading(false);
        }
    };

    const handleDelete = async (operator: OperatorDto) => {
        try {
            await adminOperatorApi.delete(operator.id);
            setOperators(prev => prev.filter(op => op.id !== operator.id));
            setDeleteModal({ show: false, operator: null });
        } catch (err) {
            setError('Ошибка удаления оператора');
        }
    };

    const getRarityStars = (rarity: number) => '★'.repeat(rarity);
    const getRarityColor = (rarity: number) => {
        const colors = { 1: 'secondary', 2: 'success', 3: 'info', 4: 'primary', 5: 'warning', 6: 'danger' };
        return colors[rarity as keyof typeof colors] || 'secondary';
    };

    if (loading) {
        return (
            <div className="text-center">
                <Spinner animation="border" />
                <p>Загрузка операторов...</p>
            </div>
        );
    }

    return (
        <div>
            <div className="d-flex justify-content-between align-items-center mb-4">
                <h2>Управление операторами</h2>
                <Link to="/admin/operators/create" className="btn btn-primary text-decoration-none">
                    <i className="bi bi-plus-circle me-2"></i>
                    Добавить оператора
                </Link>
            </div>

            {error && <Alert variant="danger">{error}</Alert>}

            <Table striped bordered hover responsive>
                <thead>
                <tr>
                    <th>ID</th>
                    <th>Имя</th>
                    <th>Редкость</th>
                    <th>Класс</th>
                    <th>Фракция</th>
                    <th>Статус</th>
                    <th>Действия</th>
                </tr>
                </thead>
                <tbody>
                {operators.map(operator => (
                    <tr key={operator.id}>
                        <td>{operator.id}</td>
                        <td>
                            <strong>{operator.name}</strong>
                        </td>
                        <td>
                            <Badge bg={getRarityColor(operator.rarity)}>
                                {getRarityStars(operator.rarity)}
                            </Badge>
                        </td>
                        <td>{operator.className}</td>
                        <td>{operator.factionName || 'Нет'}</td>
                        <td>
                            <Badge bg={operator.isGlobalReleased ? 'success' : 'warning'}>
                                {operator.releaseStatus}
                            </Badge>
                        </td>
                        <td>
                            <div className="d-flex gap-1">
                                <Button
                                    size="sm"
                                    variant="outline-primary"
                                    onClick={() => navigate(`/admin/operators/edit/${operator.id}`)}
                                >
                                    <i className="bi bi-pencil"></i>
                                </Button>
                                <Button
                                    size="sm"
                                    variant="outline-danger"
                                    onClick={() => setDeleteModal({ show: true, operator })}
                                >
                                    <i className="bi bi-trash"></i>
                                </Button>
                            </div>
                        </td>
                    </tr>
                ))}
                </tbody>
            </Table>

            {/* Модальное окно подтверждения удаления */}
            <Modal show={deleteModal.show} onHide={() => setDeleteModal({ show: false, operator: null })}>
                <Modal.Header closeButton>
                    <Modal.Title>Подтверждение удаления</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    Вы уверены, что хотите удалить оператора <strong>{deleteModal.operator?.name}</strong>?
                    <br />
                    <small className="text-muted">Это действие нельзя отменить.</small>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={() => setDeleteModal({ show: false, operator: null })}>
                        Отмена
                    </Button>
                    <Button variant="danger" onClick={() => deleteModal.operator && handleDelete(deleteModal.operator)}>
                        Удалить
                    </Button>
                </Modal.Footer>
            </Modal>
        </div>
    );
};

const CreateOperator: React.FC = () => {
    const navigate = useNavigate();

    const handleSubmit = async (data: any) => {
        await adminOperatorApi.create(data);
        navigate('/admin/operators');
    };

    return (
        <div>
            <h2 className="mb-4">Создание нового оператора</h2>
            <OperatorForm
                onSubmit={handleSubmit}
                onCancel={() => navigate('/admin/operators')}
            />
        </div>
    );
};

const EditOperator: React.FC = () => {
    const navigate = useNavigate();
    // TODO: Загрузка данных оператора по ID и передача в форму

    return (
        <div>
            <h2 className="mb-4">Редактирование оператора</h2>
            <Alert variant="info">Функция редактирования в разработке</Alert>
            <Button onClick={() => navigate('/admin/operators')}>Назад к списку</Button>
        </div>
    );
};

export default OperatorManagement;

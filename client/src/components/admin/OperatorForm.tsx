import React, {useEffect, useState} from "react";
import { Form, Button, Row, Col, Alert, Spinner } from 'react-bootstrap';
import {FactionDto, OperatorClassDto} from "../../types";
import {referenceApi} from "../../services/api";

interface OperatorFormData {
    name: string;
    rarity: number;
    operatorClassId: number;
    subClassId: number;
    factionId: number | null;
    imageUrl: string;
    description: string;
    position: string;
    cnReleaseDate: string;
    globalReleaseDate: string;
}

interface OperatorFormProps {
    initialData?: Partial<OperatorFormData>;
    onSubmit: (data: OperatorFormData) => Promise<void>;
    onCancel: () => void;
    isEditing?: boolean;
}

const OperatorForm: React.FC<OperatorFormProps> = ({
                                                       initialData,
                                                       onSubmit,
                                                       onCancel,
                                                       isEditing = false
                                                   }) => {
    const [formData, setFormData] = useState<OperatorFormData>({
        name: '',
        rarity: 3,
        operatorClassId: 1,
        subClassId: 1,
        factionId: null,
        imageUrl: '',
        description: '',
        position: 'Ranged',
        cnReleaseDate: new Date().toISOString().split('T')[0],
        globalReleaseDate: '',
        ...initialData
    });

    const [classes, setClasses] = useState<OperatorClassDto[]>([]);
    const [factions, setFactions] = useState<FactionDto[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchReferenceData = async () => {
            try {
                const [classesRes, factionsRes] = await Promise.all([
                    referenceApi.getClasses(),
                    referenceApi.getFactions()
                ]);
                setClasses(classesRes.data);
                setFactions(factionsRes.data);
            } catch (err) {
                setError('Ошибка загрузки справочных данных');
            }
        };

        fetchReferenceData();
    }, []);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        setError(null);

        try {
            await onSubmit(formData);
        } catch (err: any) {
            setError(err.response?.data?.message || 'Ошибка сохранения');
        } finally {
            setLoading(false);
        }
    };

    const handleChange = (field: keyof OperatorFormData, value: any) => {
        setFormData(prev => ({ ...prev, [field]: value }));
    };

    return (
        <Form onSubmit={handleSubmit}>
            {error && <Alert variant="danger">{error}</Alert>}

            <Row>
                <Col md={6}>
                    <Form.Group className="mb-3">
                        <Form.Label>Имя оператора *</Form.Label>
                        <Form.Control
                            type="text"
                            value={formData.name}
                            onChange={(e) => handleChange('name', e.target.value)}
                            required
                            disabled={loading}
                        />
                    </Form.Group>
                </Col>
                <Col md={6}>
                    <Form.Group className="mb-3">
                        <Form.Label>Редкость *</Form.Label>
                        <Form.Select
                            value={formData.rarity}
                            onChange={(e) => handleChange('rarity', Number(e.target.value))}
                            required
                            disabled={loading}
                        >
                            {[1, 2, 3, 4, 5, 6].map(rarity => (
                                <option key={rarity} value={rarity}>
                                    {'★'.repeat(rarity)} ({rarity} звезд)
                                </option>
                            ))}
                        </Form.Select>
                    </Form.Group>
                </Col>
            </Row>

            <Row>
                <Col md={6}>
                    <Form.Group className="mb-3">
                        <Form.Label>Класс *</Form.Label>
                        <Form.Select
                            value={formData.operatorClassId}
                            onChange={(e) => handleChange('operatorClassId', Number(e.target.value))}
                            required
                            disabled={loading}
                        >
                            {classes.map(cls => (
                                <option key={cls.id} value={cls.id}>
                                    {cls.name}
                                </option>
                            ))}
                        </Form.Select>
                    </Form.Group>
                </Col>
                <Col md={6}>
                    <Form.Group className="mb-3">
                        <Form.Label>Фракция</Form.Label>
                        <Form.Select
                            value={formData.factionId || ''}
                            onChange={(e) => handleChange('factionId', e.target.value ? Number(e.target.value) : null)}
                            disabled={loading}
                        >
                            <option value="">Без фракции</option>
                            {factions.map(faction => (
                                <option key={faction.id} value={faction.id}>
                                    {faction.name}
                                </option>
                            ))}
                        </Form.Select>
                    </Form.Group>
                </Col>
            </Row>

            <Row>
                <Col md={6}>
                    <Form.Group className="mb-3">
                        <Form.Label>Позиция *</Form.Label>
                        <Form.Select
                            value={formData.position}
                            onChange={(e) => handleChange('position', e.target.value)}
                            required
                            disabled={loading}
                        >
                            <option value="Melee">Ближний бой (Melee)</option>
                            <option value="Ranged">Дальний бой (Ranged)</option>
                        </Form.Select>
                    </Form.Group>
                </Col>
                <Col md={6}>
                    <Form.Group className="mb-3">
                        <Form.Label>URL изображения</Form.Label>
                        <Form.Control
                            type="url"
                            value={formData.imageUrl}
                            onChange={(e) => handleChange('imageUrl', e.target.value)}
                            placeholder="/images/operators/operator-name.jpg"
                            disabled={loading}
                        />
                    </Form.Group>
                </Col>
            </Row>

            <Form.Group className="mb-3">
                <Form.Label>Описание</Form.Label>
                <Form.Control
                    as="textarea"
                    rows={3}
                    value={formData.description}
                    onChange={(e) => handleChange('description', e.target.value)}
                    disabled={loading}
                />
            </Form.Group>

            <Row>
                <Col md={6}>
                    <Form.Group className="mb-3">
                        <Form.Label>Дата релиза в CN *</Form.Label>
                        <Form.Control
                            type="date"
                            value={formData.cnReleaseDate}
                            onChange={(e) => handleChange('cnReleaseDate', e.target.value)}
                            required
                            disabled={loading}
                        />
                    </Form.Group>
                </Col>
                <Col md={6}>
                    <Form.Group className="mb-3">
                        <Form.Label>Дата релиза Global</Form.Label>
                        <Form.Control
                            type="date"
                            value={formData.globalReleaseDate}
                            onChange={(e) => handleChange('globalReleaseDate', e.target.value)}
                            disabled={loading}
                        />
                    </Form.Group>
                </Col>
            </Row>

            <div className="d-flex gap-2">
                <Button type="submit" variant="primary" disabled={loading}>
                    {loading ? (
                        <>
                            <Spinner size="sm" className="me-2" />
                            {isEditing ? 'Сохранение...' : 'Создание...'}
                        </>
                    ) : (
                        isEditing ? 'Сохранить изменения' : 'Создать оператора'
                    )}
                </Button>
                <Button type="button" variant="secondary" onClick={onCancel} disabled={loading}>
                    Отмена
                </Button>
            </div>
        </Form>
    );
};

export default OperatorForm;
import React, { useState, useEffect } from 'react';
import { Form, Button, Row, Col, Alert, Spinner, Card, Image } from 'react-bootstrap';
import { referenceApi, fileUploadApi } from '../../services/api';
import { OperatorClassDto, FactionDto, SubClassDto } from '../../types';

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
    const [subClasses, setSubClasses] = useState<SubClassDto[]>([]);
    const [factions, setFactions] = useState<FactionDto[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [imageFile, setImageFile] = useState<File | null>(null);
    const [imagePreview, setImagePreview] = useState<string>('');
    const [uploadingImage, setUploadingImage] = useState(false);

    // Загрузка справочных данных
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

    // Загрузка субклассов при изменении класса
    useEffect(() => {
        const fetchSubClasses = async () => {
            if (formData.operatorClassId) {
                try {
                    const response = await referenceApi.getSubClassesByClass(formData.operatorClassId);
                    setSubClasses(response.data);

                    // Если текущий субкласс не подходит к новому классу, сбрасываем
                    const validSubClass = response.data.find(sc => sc.id === formData.subClassId);
                    if (!validSubClass && response.data.length > 0) {
                        setFormData(prev => ({ ...prev, subClassId: response.data[0].id }));
                    }
                } catch (err) {
                    console.error('Error fetching subclasses:', err);
                    setSubClasses([]);
                }
            }
        };

        fetchSubClasses();
    }, [formData.operatorClassId]);

    // Обработка выбора файла изображения
    const handleImageChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const file = e.target.files?.[0];
        if (file) {
            setImageFile(file);

            // Создаем превью
            const reader = new FileReader();
            reader.onload = (e) => {
                setImagePreview(e.target?.result as string);
            };
            reader.readAsDataURL(file);
        }
    };

    // Загрузка изображения на сервер
    const uploadImage = async () => {
        if (!imageFile || !formData.name.trim()) {
            setError('Выберите изображение и введите имя оператора');
            return;
        }

        setUploadingImage(true);
        try {
            const response = await fileUploadApi.uploadOperatorImage(imageFile, formData.name);
            setFormData(prev => ({ ...prev, imageUrl: response.data.imageUrl }));
            setError(null);
        } catch (err: any) {
            setError(err.response?.data?.message || 'Ошибка загрузки изображения');
        } finally {
            setUploadingImage(false);
        }
    };

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

            {/* Строка 1: Имя + Редкость */}
            <Row className="mb-3">
                <Col md={8}>
                    <Form.Group>
                        <Form.Label>Имя оператора *</Form.Label>
                        <Form.Control
                            type="text"
                            value={formData.name}
                            onChange={(e) => handleChange('name', e.target.value)}
                            required
                            disabled={loading}
                            placeholder="Введите имя оператора"
                        />
                    </Form.Group>
                </Col>
                <Col md={4}>
                    <Form.Group>
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

            {/* Строка 2: Класс + Подкласс */}
            <Row className="mb-3">
                <Col md={6}>
                    <Form.Group>
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
                    <Form.Group>
                        <Form.Label>Подкласс *</Form.Label>
                        <Form.Select
                            value={formData.subClassId}
                            onChange={(e) => handleChange('subClassId', Number(e.target.value))}
                            required
                            disabled={loading || subClasses.length === 0}
                        >
                            {subClasses.length === 0 ? (
                                <option value="">Выберите класс</option>
                            ) : (
                                subClasses.map(subClass => (
                                    <option key={subClass.id} value={subClass.id}>
                                        {subClass.name}
                                    </option>
                                ))
                            )}
                        </Form.Select>
                        {subClasses.length === 0 && formData.operatorClassId && (
                            <Form.Text className="text-muted">
                                Загрузка подклассов...
                            </Form.Text>
                        )}
                    </Form.Group>
                </Col>
            </Row>

            {/* Строка 3: Фракция + Позиция */}
            <Row className="mb-3">
                <Col md={6}>
                    <Form.Group>
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
                <Col md={6}>
                    <Form.Group>
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
            </Row>

            {/* Строка 4: Изображение с превью */}
            <Row className="mb-3">
                <Col md={8}>
                    <Form.Group>
                        <Form.Label>Изображение оператора</Form.Label>
                        <Form.Control
                            type="file"
                            accept="image/*"
                            onChange={handleImageChange}
                            disabled={loading}
                        />
                        <Form.Text className="text-muted">
                            Поддерживаются JPG, PNG, WebP. Максимум 15MB.
                            {formData.name && ` Файл будет сохранен как: ${formData.name.toLowerCase().replace(/\s+/g, '-')}.jpg`}
                        </Form.Text>
                        {imageFile && (
                            <div className="mt-2">
                                <Button
                                    variant="outline-primary"
                                    size="sm"
                                    onClick={uploadImage}
                                    disabled={uploadingImage || !formData.name.trim()}
                                >
                                    {uploadingImage ? (
                                        <>
                                            <Spinner size="sm" className="me-2" />
                                            Загрузка...
                                        </>
                                    ) : (
                                        'Загрузить изображение'
                                    )}
                                </Button>
                            </div>
                        )}
                    </Form.Group>
                </Col>
                <Col md={4}>
                    <Form.Label>Превью</Form.Label>
                    <Card style={{ height: '200px' }}>
                        <Card.Body className="d-flex align-items-center justify-content-center p-2">
                            {imagePreview ? (
                                <Image
                                    src={imagePreview}
                                    alt="Превью"
                                    style={{ maxWidth: '100%', maxHeight: '100%', objectFit: 'contain' }}
                                />
                            ) : formData.imageUrl ? (
                                <Image
                                    src={`http://localhost:5000${formData.imageUrl}`}
                                    alt="Текущее изображение"
                                    style={{ maxWidth: '100%', maxHeight: '100%', objectFit: 'contain' }}
                                />
                            ) : (
                                <div className="text-muted text-center">
                                    <i className="bi bi-image" style={{ fontSize: '2rem' }}></i>
                                    <br />
                                    Нет изображения
                                </div>
                            )}
                        </Card.Body>
                    </Card>
                </Col>
            </Row>

            {/* Строка 5: Описание */}
            <Row className="mb-3">
                <Col>
                    <Form.Group>
                        <Form.Label>Описание</Form.Label>
                        <Form.Control
                            as="textarea"
                            rows={3}
                            value={formData.description}
                            onChange={(e) => handleChange('description', e.target.value)}
                            disabled={loading}
                            placeholder="Описание оператора..."
                        />
                    </Form.Group>
                </Col>
            </Row>

            {/* Строка 6: Даты релиза */}
            <Row className="mb-4">
                <Col md={6}>
                    <Form.Group>
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
                    <Form.Group>
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

            {/* Кнопки */}
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

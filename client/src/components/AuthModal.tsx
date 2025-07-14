import React, {useState} from "react";
import {useAuth} from "../contexts/AuthContext";
import {LoginRequest, RegisterRequest} from "../types";
import {Alert, Button, Form, Modal, Tab, Tabs} from "react-bootstrap";

interface AuthModalProps {
    show: boolean;
    onHide: () => void;
}

const AuthModal: React.FC<AuthModalProps> = ({show, onHide}) => {
    const {login, register} = useAuth();
    const [activeTab, setActiveTab] = useState<'login' | 'register'>('login');
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const [loginData, setLoginData] = useState<LoginRequest>({
        username: '',
        password: ''
    });

    const [registerData, setRegisterData] = useState<RegisterRequest>({
        username: '',
        email: '',
        password: '',
        confirmPassword: ''
    });

    const handleLogin = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        setError(null);

        if (registerData.password !== registerData.confirmPassword) {
            setError('Пароли не совпадают');
            setLoading(false);
            return;
        }

        try {
            await login(loginData);
            onHide();
            setLoginData({username: '', password: ''});
        } catch (err: any) {
            setError(err.response?.data?.message || 'Ошибка регистрации');
        } finally {
            setLoading(false);
        }
    }

    const handleRegister = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        setError(null);

        if (registerData.password !== registerData.confirmPassword) {
            setError('Пароли не совпадают');
            setLoading(false);
            return;
        }

        try {
            await register(registerData);
            onHide();
            // Сброс формы
            setRegisterData({ username: '', email: '', password: '', confirmPassword: '' });
        } catch (err: any) {
            setError(err.response?.data?.message || 'Ошибка регистрации');
        } finally {
            setLoading(false);
        }
    };

    const handleClose = () => {
        setError(null);
        setLoginData({username: '', password: ''});
        setRegisterData({username: '', email: '', password: '', confirmPassword: ''});
        onHide();
    };

    return (
        <Modal show={show} onHide={handleClose} centered>
            <Modal.Header closeButton>
                <Modal.Title>
                    <i className="bi bi-shield-check me-2"></i>
                    Аутентификация
                </Modal.Title>
            </Modal.Header>
            <Modal.Body>
                {error && <Alert variant="danger">{error}</Alert>}

                <Tabs
                    activeKey={activeTab}
                    onSelect={(k) => setActiveTab(k as 'login' | 'register')}
                    className="mb-3"
                >
                    <Tab eventKey="login" title="Вход">
                        <Form onSubmit={handleLogin}>
                            <Form.Group className="mb-3">
                                <Form.Label>Имя пользователя</Form.Label>
                                <Form.Control
                                    type="text"
                                    value={loginData.username}
                                    onChange={(e) => setLoginData({...loginData, username: e.target.value})}
                                    required
                                    disabled={loading}
                                />
                            </Form.Group>

                            <Form.Group className="mb-3">
                                <Form.Label>Пароль</Form.Label>
                                <Form.Control
                                    type="password"
                                    value={loginData.password}
                                    onChange={(e) => setLoginData({...loginData, password: e.target.value})}
                                    required
                                    disabled={loading}
                                />
                            </Form.Group>

                            <div className="d-grid gap-2">
                                <Button type="submit" variant="primary" disabled={loading}>
                                    {loading ? (
                                        <>
                                            <span className="spinner-border spinner-border-sm me-2"/>
                                            Вход ...
                                        </>
                                    ) : (
                                        'Войти'
                                    )}
                                </Button>
                            </div>
                        </Form>
                    </Tab>

                    <Tab eventKey="register" title="Регистрация">
                        <Form onSubmit={handleRegister}>
                            <Form.Group className="mb-3">
                                <Form.Label>Имя пользователя</Form.Label>
                                <Form.Control
                                    type="text"
                                    value={registerData.username}
                                    onChange={(e) => setRegisterData({ ...registerData, username: e.target.value })}
                                    required
                                    disabled={loading}
                                />
                            </Form.Group>

                            <Form.Group className="mb-3">
                                <Form.Label>Email</Form.Label>
                                <Form.Control
                                    type="email"
                                    value={registerData.email}
                                    onChange={(e) => setRegisterData({ ...registerData, email: e.target.value })}
                                    required
                                    disabled={loading}
                                />
                            </Form.Group>

                            <Form.Group className="mb-3">
                                <Form.Label>Пароль</Form.Label>
                                <Form.Control
                                    type="password"
                                    value={registerData.password}
                                    onChange={(e) => setRegisterData({ ...registerData, password: e.target.value })}
                                    required
                                    disabled={loading}
                                    minLength={6}
                                />
                            </Form.Group>

                            <Form.Group className="mb-3">
                                <Form.Label>Подтверждение пароля</Form.Label>
                                <Form.Control
                                    type="password"
                                    value={registerData.confirmPassword}
                                    onChange={(e) => setRegisterData({ ...registerData, confirmPassword: e.target.value })}
                                    required
                                    disabled={loading}
                                />
                            </Form.Group>

                            <div className="d-grid gap-2">
                                <Button type="submit" variant="success" disabled={loading}>
                                    {loading ? (
                                        <>
                                            <span className="spinner-border spinner-border-sm me-2" />
                                            Регистрация...
                                        </>
                                    ) : (
                                        'Зарегистрироваться'
                                    )}
                                </Button>
                            </div>
                        </Form>
                    </Tab>
                </Tabs>
            </Modal.Body>
        </Modal>
    )
}

export default AuthModal;
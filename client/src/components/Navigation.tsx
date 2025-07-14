import React, {useState} from "react";
import {Button, Container, Nav, Navbar, NavDropdown} from "react-bootstrap";
import {Link, useLocation} from "react-router-dom";
import {useAuth} from "../contexts/AuthContext";
import AuthModal from "./AuthModal";

const Navigation: React.FC = () => {
    const location = useLocation();
    const {user, logout, isAuthenticated, isAdmin} = useAuth();
    const [showAuthModal, setShowAuthModal] = useState(false);

    const handleLogout = () => {
        logout();
    }

    return (
        <>
            <Navbar bg="dark" variant="dark" expand="lg" sticky="top">
                <Container>
                    <Navbar.Brand as={Link} to="/" className="text-decoration-none">
                        <i className="bi bi-shield-check me-2"></i>
                        Arknights Operators
                    </Navbar.Brand>

                    <Navbar.Toggle aria-controls="basic-navbar-nav"/>
                    <Navbar.Collapse id="basic-navbar-nav">
                        <Nav className="me-auto">
                            <Nav.Link
                                as={Link}
                                to="/"
                                active={location.pathname === '/'}
                                className="text-decoration-none"
                            >
                                Главная
                            </Nav.Link>
                            <Nav.Link
                                as={Link}
                                to="/operators"
                                active={location.pathname.startsWith('/operators')}
                                className="text-decoration-none"
                            >
                                Операторы
                            </Nav.Link>

                            {/* Админ-панель */}
                            {isAdmin && (
                                <Nav.Link
                                    as={Link}
                                    to="/admin"
                                    active={location.pathname.startsWith('/admin')}
                                    className="text-decoration-none"
                                >
                                    <i className="bi bi-gear me-1"></i>
                                    Админ-панель
                                </Nav.Link>
                            )}

                            {/* Правая часть навигации */}
                            <Nav>
                                {isAuthenticated ? (
                                    <NavDropdown
                                        title={
                                            <>
                                                <i className="bi bi-person-circle me-1"></i>
                                                {user?.username}
                                                {isAdmin && <span className="badge bg-danger ms-1">Admin</span>}
                                            </>
                                        }
                                        id="user-nav-dropdown"
                                    >
                                        <NavDropdown.Item disabled>
                                            <small className="text-muted">
                                                {user?.email}
                                            </small>
                                        </NavDropdown.Item>
                                        <NavDropdown.Divider/>
                                        <NavDropdown.Item onClick={handleLogout}>
                                            <i className="bi bi-box-arrow-right me-2"></i>
                                            Выйти
                                        </NavDropdown.Item>
                                    </NavDropdown>
                                ) : (
                                    <Button
                                        variant="outline-light"
                                        onClick={() => setShowAuthModal(true)}
                                    >
                                        <i className="bi bi-person me-1"></i>
                                        Войти
                                    </Button>
                                )}
                            </Nav>
                        </Nav>
                    </Navbar.Collapse>
                </Container>
            </Navbar>

            {/* Модальное окно аутентификации */}
            <AuthModal
                show={showAuthModal}
                onHide={() => setShowAuthModal(false)}
            />
        </>
    );
};

export default Navigation;

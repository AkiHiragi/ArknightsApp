import React from "react";
import { Container, Nav, Navbar } from "react-bootstrap";
import { Link, useLocation } from "react-router-dom";

const Navigation: React.FC = () => {
    const location = useLocation();

    return (
        <Navbar bg="dark" variant="dark" expand="lg" sticky="top">
            <Container>
                <Navbar.Brand as={Link} to="/" className="text-decoration-none">
                    <i className="bi bi-shield-check me-2"></i>
                    Arknights Operators
                </Navbar.Brand>

                <Navbar.Toggle aria-controls="basic-navbar-nav" />
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
                    </Nav>
                </Navbar.Collapse>
            </Container>
        </Navbar>
    );
};

export default Navigation;

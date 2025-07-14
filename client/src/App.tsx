import React from 'react';
import {BrowserRouter as Router, Routes, Route} from "react-router-dom";
import {Container} from "react-bootstrap";
import {AuthProvider} from "./contexts/AuthContext";
import Navigation from "./components/Navigation";
import Home from "./pages/Home";
import OperatorList from "./pages/OperatorList";
import OperatorDetail from "./pages/OperatorDetail";
import ProtectedRoute from "./components/ProtectedRoute";
import AdminPanel from "./pages/AdminPanel";

function App() {
    return (
        <AuthProvider>
            <Router>
                <div className="App">
                    <Navigation/>
                    <Container className="mt-4">
                        <Routes>
                            <Route path="/" element={<Home/>}/>
                            <Route path="/operators" element={<OperatorList/>}/>
                            <Route path="/operators/:id" element={<OperatorDetail/>}/>

                            {/* Защищенный роут для админ-панели */}
                            <Route
                                path="/admin/*"
                                element={
                                    <ProtectedRoute requiredRole="Admin">
                                        <AdminPanel/>
                                    </ProtectedRoute>
                                }
                            />
                        </Routes>
                    </Container>
                </div>
            </Router>
        </AuthProvider>
    );
}

export default App;

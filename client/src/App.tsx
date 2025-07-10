import React from 'react';
import {BrowserRouter as Router, Routes, Route} from "react-router-dom";
import {Container} from "react-bootstrap";
import Navigation from "./components/Navigation";
import Home from "./pages/Home";
import OperatorList from "./pages/OperatorList";
import OperatorDetail from "./pages/OperatorDetail";

function App() {
    return (
        <Router>
            <div className="App">
                <Navigation/>
                <Container className="mt-4">
                    <Routes>
                        <Route path="/" element={<Home/>}/>
                        <Route path="/operators" element={<OperatorList/>}/>
                        <Route path="/operators/:id" element={<OperatorDetail/>}/>
                    </Routes>
                </Container>
            </div>
        </Router>
    );
}

export default App;

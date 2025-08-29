import {OperatorList} from "./components/OperatorList.tsx";
import {useState} from "react";
import type {Operator} from "./types/operator.ts";
import {OperatorManager} from "./components/OperatorManager.tsx";

function App() {
    const [currentView, setCurrentView] = useState<'list' | 'manager'>('list');
    const [operators, setOperators] = useState<Operator[]>([]);

    const handleOperatorUpdate = (updateOperator: Operator) => {
        setOperators(prev =>
            prev.map(op => op.id === updateOperator.id ? updateOperator : op)
        );
    };

    const handleOperatorsLoaded = (loadedOperators: Operator[]) => {
        setOperators(loadedOperators);
    };

    return (
        <div className="min-h-screen bg-gray-100">
            <header className="bg-blue-600 text-white p-4">
                <div className="container mx-auto flex justify-between items-center">
                    <h1 className="text-2xl font-bold">Arknights Operators</h1>
                    <nav className="space-x-4">
                        <button
                            onClick={() => setCurrentView('list')}
                            className={`px-4 py-2 rounded ${currentView === 'list' ? 'bg-blue-800' : 'bg-blue-500 hover:bg-blue-700'}`}
                        >
                            Operators
                        </button>
                        <button
                            onClick={() => setCurrentView('manager')}
                            className={`px-4 py-2 rounded ${currentView === 'manager' ? 'bg-blue-800' : 'bg-blue-500 hover:bg-blue-700'}`}
                        >
                            Manager
                        </button>
                    </nav>
                </div>
            </header>

            <main className="container mx-auto p-4">
                {currentView === 'list' ? (
                    <OperatorList onOperatorsLoaded={handleOperatorsLoaded}/>
                ) : (
                    <OperatorManager 
                        operators={operators} 
                        onOperatorUpdate={handleOperatorUpdate}
                    />
                )}
            </main>
        </div>
    )
}

export default App

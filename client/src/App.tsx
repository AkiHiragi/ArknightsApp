import {OperatorList} from "./components/OperatorList.tsx";

function App() {
    return (
        <div className="min-h-screen bg-gray-100">
            <header className="bg-blue-600 text-white p-4">
                <h1 className="text-2xl font-bold">Arknights Operators</h1>
            </header>
            <main className="container mx-auto p-4">
                <OperatorList/>
            </main>
        </div>
    )
}

export default App

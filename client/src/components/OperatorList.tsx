import {useEffect, useState} from "react";
import type {Operator} from "../types/operator.ts";
import {operatorService} from "../services/OperatorService.ts";
import {OperatorCard} from "./OperatorCard.tsx";

export function OperatorList() {
    const [operators, setOperators] = useState<Operator[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchOperators = async () => {
            try {
                const data = await operatorService.getAll();
                setOperators(data);
            } catch (error) {
                console.error('Failed to fetch operators:', error);
            } finally {
                setLoading(false);
            }
        };

        void fetchOperators();
    }, []);

    if (loading) return <p className="text-gray-600">Loading operators...</p>;

    return (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
            {operators.map(operator => (
                <OperatorCard key={operator.id} operator={operator}/>
            ))}
        </div>
    );
}
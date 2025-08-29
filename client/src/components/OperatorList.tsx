import {useEffect, useState} from "react";
import type {Operator} from "../types/operator.ts";
import {operatorService} from "../services/OperatorService.ts";
import {OperatorCard} from "./OperatorCard.tsx";

interface Props {
    onOperatorsLoaded?: (operators: Operator[]) => void;
}

export function OperatorList({onOperatorsLoaded}: Props) {
    const [operators, setOperators] = useState<Operator[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchOperators = async () => {
            try {
                const data = await operatorService.getAll();
                setOperators(data);
                onOperatorsLoaded?.(data);
            } catch (error) {
                console.error('Failed to fetch operators:', error);
            } finally {
                setLoading(false);
            }
        };

        void fetchOperators();
    }, [onOperatorsLoaded]);

    if (loading) return <p className="text-gray-600">Loading operators...</p>;

    return (
        <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 xl:grid-cols-6 2xl:grid-cols-8 gap-4 justify-items-center">
            {operators.map(operator => (
                <OperatorCard key={operator.id} operator={operator} />
            ))}
        </div>
    );
}
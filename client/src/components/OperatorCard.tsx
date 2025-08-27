import type {Operator} from "../types/operator.ts";

interface Props {
    operator: Operator;
}

export function OperatorCard({operator}: Props) {
    const rarityStars = '★'.repeat(operator.rarity);

    return (
        <div className="bg-white rounded-lg shadow-md p-4 hover:shadow-lg transition-shadow">
            <div className="flex justify-between items-start mb-2">
                <h3 className="text-lg font-semibold text-gray-800">{operator.name}</h3>
                <span className="text-yellow-500 font-bold">{rarityStars}</span>
            </div>
            <div className="space-y-1 text-sm text-gray-600">
                <p><span className="font-medium">Class:</span> {operator.class}</p>
                <p><span className="font-medium">Subclass:</span> {operator.subclass}</p>
            </div>

            <p className="mt-3 text-gray-700 text-sm">{operator.description}</p>
        </div>
    );
}
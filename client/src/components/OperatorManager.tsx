import type {Operator} from "../types/operator.ts";
import {useState} from "react";
import {ImageCropper} from "./ImageCropper.tsx";
import {operatorService} from "../services/OperatorService.ts";

interface Props {
    operators: Operator[];
    onOperatorUpdate: (operator: Operator) => void;
}

export function OperatorManager({operators, onOperatorUpdate}: Props) {
    const [selectedOperator, setSelectedOperator] = useState<Operator | null>(null);
    const [showCropper, setShowCropper] = useState(false);

    const handleImageSuccess = async () => {
        if (!selectedOperator) return;

        try {
            const updatedOperator = await operatorService.getById(selectedOperator.id);
            onOperatorUpdate(updatedOperator);
            setShowCropper(false);
            alert('Images processed successfully!');
        } catch (error) {
            console.error('Failed to reload operator:', error);
            alert('Images processed, but failed to reload data');
        }
    }

    return (
        <div className="space-y-6">
            <h2 className="text-2xl font-bold">Operator Manager</h2>

            {/* Список операторов */}
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                {operators.map(operator => (
                    <div key={operator.id} className="border rounded-lg p-4">
                        <div className="flex items-center space-x-3 mb-3">
                            {operator.avatarUrl ? (
                                <img
                                    src={operator.avatarUrl}
                                    alt={operator.name}
                                    className="w-12 h-12 rounded-full object-cover"
                                />
                            ) : (
                                <div className="w-12 h-12 bg-gray-200 rounded-full flex items-center justify-center">
                                    <span className="text-gray-400 text-xs">No img</span>
                                </div>
                            )}
                            <div>
                                <h3 className="font-semibold">{operator.name}</h3>
                                <p className="text-sm text-gray-600">{'★'.repeat(operator.rarity)}</p>
                            </div>
                        </div>

                        <button
                            onClick={() => {
                                setSelectedOperator(operator);
                                setShowCropper(true);
                            }}
                            className="w-full bg-blue-600 text-white py-2 px-4 rounded hover:bg-blue-700"
                        >
                            Upload E0 Art
                        </button>
                    </div>
                ))}
            </div>

            {/* Modal для cropper */}
            {showCropper && selectedOperator && (
                <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center p-4 z-50">
                    <div className="bg-white rounded-lg p-6 max-w-4xl w-full max-h-[90vh] overflow-y-auto">
                        <div className="flex justify-between items-center mb-4">
                            <h3 className="text-xl font-bold">Upload E0 Art for {selectedOperator.name}</h3>
                            <button
                                onClick={() => setShowCropper(false)}
                                className="text-gray-500 hover:text-gray-700"
                            >
                                ✕
                            </button>
                        </div>

                        <ImageCropper
                            operatorId={selectedOperator.id}
                            onSuccess={handleImageSuccess}
                        />
                    </div>
                </div>
            )}
        </div>
    );
}
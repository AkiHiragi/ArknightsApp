import {useEffect, useState} from "react";
import type {Class, Subclass} from "../types/class.ts";
import {classService} from "../services/ClassService.ts";
import type {Operator} from "../types/operator.ts";

export function ClassList() {
    const [classes, setClasses] = useState<Class[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [selectedClass, setSelectedClass] = useState<Class | null>(null);
    const [subclasses, setSubclasses] = useState<Subclass[]>([]);
    const [classOperators, setClassOperators] = useState<Operator[]>([]);


    useEffect(() => {
        void loadClasses();
    }, []);

    const loadClasses = async () => {
        try {
            setLoading(true);
            const data = await classService.getAll();
            setClasses(data);
        } catch (err) {
            setError('Failed to load classes');
            console.error('Error loading classes', err);
        } finally {
            setLoading(false);
        }
    }

    const handleClassClick = async (operatorClass: Class) => {
        setSelectedClass(operatorClass);
        try {
            const [subclassData, operatorData] = await Promise.all([
                classService.getSubclasses(operatorClass.id),
                classService.getOperators(operatorClass.id)
            ]);
            setSubclasses(subclassData);
            setClassOperators(operatorData);
        } catch (err) {
            console.error('Error loading class data:', err);
        }
    };


    if (loading) return <div className="text-center py-8">Loading classes...</div>
    if (error) return <div className="text-center py-8 text-red-600">{error}</div>

    return (
        <div className="space-y-6">
            <h2 className="text-2xl font-bold">Operator classes</h2>

            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4">
                {classes.map(operatorClass => (
                    <div
                        key={operatorClass.id}
                        className="bg-white rounded-lg shadow-md p-6 hover:shadow-lg transition-shadow cursor-pointer"
                        onClick={() => handleClassClick(operatorClass)}
                    >
                        <h3 className="text-xl font-semibold mb-2">{operatorClass.name}</h3>
                        <p className="text-gray-600 text-sm">{operatorClass.description}</p>
                    </div>
                ))}
            </div>
            
            {selectedClass && (
                <div className="mt-8 bg-white rounded-lg shadow-md p-6">
                    <div className="flex justify-between items-center mb-4">
                        <h3 className="text-xl font-semibold">{selectedClass.name}</h3>
                        <button
                            onClick={() => setSelectedClass(null)}
                            className="text-gray-500 hover:text-gray-700 text-xl"
                        >
                            ✕
                        </button>
                    </div>

                    <p className="text-gray-600 mb-6">{selectedClass.description}</p>

                    <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
                        <div>
                            <h4 className="font-semibold mb-3">Subclasses ({subclasses.length})</h4>
                            <div className="space-y-2">
                                {subclasses.map(subclass => (
                                    <div key={subclass.id} className="bg-gray-50 p-3 rounded">
                                        <div className="font-medium">{subclass.name}</div>
                                        <div className="text-sm text-gray-600">{subclass.description}</div>
                                    </div>
                                ))}
                            </div>
                        </div>

                        <div>
                            <h4 className="font-semibold mb-3">Operators ({classOperators.length})</h4>
                            <div className="space-y-2">
                                {classOperators.map(operator => (
                                    <div key={operator.id}
                                         className="bg-gray-50 p-3 rounded flex items-center space-x-3">
                                        {operator.avatarUrl && (
                                            <img
                                                src={operator.avatarUrl}
                                                alt={operator.name}
                                                className="w-8 h-8 rounded-full"
                                            />
                                        )}
                                        <div>
                                            <div className="font-medium">{operator.name}</div>
                                            <div className="text-sm text-gray-600">
                                                {'★'.repeat(operator.rarity)} • {operator.subclassName}
                                            </div>
                                        </div>
                                    </div>
                                ))}
                            </div>
                        </div>
                    </div>
                </div>
            )}
        </div>
    )
}
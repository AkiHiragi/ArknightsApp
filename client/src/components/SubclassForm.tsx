import type {CreateSubclass, Subclass} from "../types/class.ts";
import {useEffect, useState} from "react";
import * as React from "react";
import {subclassService} from "../services/SubclassService.ts";

interface Props {
    classId: number;
    editingSubclass?: Subclass | null;
    onSuccess: () => void;
    onCancel: () => void;
}

export function SubclassForm({classId, editingSubclass, onSuccess, onCancel}: Props) {
    const [formData, setFormData] = useState<CreateSubclass>({
        name: '',
        description: '',
        classId: classId
    });
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        if (editingSubclass) {
            setFormData({
                name: editingSubclass.name,
                description: editingSubclass.description,
                classId: editingSubclass.classId
            });
        }
    }, [editingSubclass]);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        try {
            if (editingSubclass) {
                await subclassService.update(editingSubclass.id, formData);
            } else {
                await subclassService.create(formData);
            }
            onSuccess();
        } catch (error) {
            console.error('Error saving subclass:', error);
            alert('Failed to save subclass');
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="bg-white p-4 rounded-lg border">
            <h5 className="font-semibold mb-3">
                {editingSubclass ? 'Edit Subclass' : 'Add New Subclass'}
            </h5>

            <form onSubmit={handleSubmit} className="space-y-3">
                <div>
                    <label className="block text-sm font-medium mb-1">Name</label>
                    <input
                        type="text"
                        value={formData.name}
                        onChange={(e) => setFormData({...formData, name: e.target.value})}
                        className="w-full px-3 py-2 border rounded-md"
                        required
                    />
                </div>

                <div>
                    <label className="block text-sm font-medium mb-1">Description</label>
                    <textarea
                        value={formData.description}
                        onChange={(e) => setFormData({...formData, description: e.target.value})}
                        className="w-full px-3 py-2 border rounded-md"
                        rows={2}
                    />
                </div>

                <div className="flex space-x-2">
                    <button
                        type="submit"
                        disabled={loading}
                        className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700 disabled:opacity-50"
                    >
                        {loading ? 'Saving...' : (editingSubclass ? 'Update' : 'Create')}
                    </button>
                    <button
                        type="button"
                        onClick={onCancel}
                        className="bg-gray-300 text-gray-700 px-4 py-2 rounded hover:bg-gray-400"
                    >
                        Cancel
                    </button>
                </div>
            </form>
        </div>
    );
}
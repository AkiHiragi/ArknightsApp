import {useRef, useState} from "react";
import ReactCrop, {type Crop} from "react-image-crop";
import 'react-image-crop/dist/ReactCrop.css';
import * as React from "react";
import axios from "axios";

interface Props {
    operatorId: number;
    onSuccess: () => void;
}

export function ImageCropper({operatorId, onSuccess}: Props) {
    const [selectedFile, setSelectedFile] = useState<File | null>(null);
    const [imageSrc, setImageSrc] = useState<string>('');
    const [cropMode, setCropMode] = useState<'avatar' | 'preview'>('avatar');
    const [avatarCrop, setAvatarCrop] = useState<Crop>({unit: 'px', x: 50, y: 50, width: 100, height: 100});
    const [previewCrop, setPreviewCrop] = useState<Crop>({unit: 'px', x: 200, y: 50, width: 200, height: 300});
    const [loading, setLoading] = useState(false);
    const imgRef = useRef<HTMLImageElement>(null);

    const handleFileSelect = (e: React.ChangeEvent<HTMLInputElement>) => {
        const file = e.target.files?.[0];
        if (!file) return;

        setSelectedFile(file);
        const reader = new FileReader();
        reader.onload = () => setImageSrc(reader.result as string);
        reader.readAsDataURL(file);
    };

    const convertToPixels = (crop: Crop) => {
        const img = imgRef.current!;
        const scaleX = img.naturalWidth / img.width;
        const scaleY = img.naturalHeight / img.height;

        return {
            x: Math.round(crop.x * scaleX),
            y: Math.round(crop.y * scaleY),
            width: Math.round(crop.width * scaleX),
            height: Math.round(crop.height * scaleY)
        };
    };

    const handleProcess = async () => {
        if (!selectedFile || !imgRef.current) return;

        const avatarPixels = convertToPixels(avatarCrop);
        const previewPixels = convertToPixels(previewCrop);

        const formData = new FormData();
        formData.append('file', selectedFile);

        setLoading(true);
        try {
            await axios.post(`/api/FileUpload/process-e0/${operatorId}`,formData,{
                params:{
                    avatarX:avatarPixels.x,
                    avatarY:avatarPixels.y,
                    avatarSize:avatarPixels.width,
                    previewX:previewPixels.x,
                    previewY:previewPixels.y,
                    previewWidth:previewPixels.width,
                    previewHeight:previewPixels.height
                }
            });
            
            onSuccess();
        } catch (error) {
            alert(`Upload failed: ${error}`);
        } finally {
            setLoading(false);
        }
    };

    const currentCrop = cropMode === 'avatar' ? avatarCrop : previewCrop;
    const currentSetter = cropMode === 'avatar' ? setAvatarCrop : setPreviewCrop;
    const aspect = cropMode === 'avatar' ? 1 : 2 / 3;

    return (
        <div className="space-y-4">
            <input
                type="file"
                accept="image/*"
                onChange={handleFileSelect}
                className="block w-full text-sm file:mr-4 file:py-2 file:px-4 file:rounded-full file:border-0 file:bg-blue-50 file:text-blue-700"
            />

            {imageSrc && (
                <>
                    <div className="flex space-x-4">
                        {['avatar', 'preview'].map(mode => (
                            <button
                                key={mode}
                                onClick={() => setCropMode(mode as 'avatar' | 'preview')}
                                className={`px-4 py-2 rounded ${cropMode === mode ? 'bg-blue-500 text-white' : 'bg-gray-200'}`}
                            >
                                {mode === 'avatar' ? '🔴 Avatar (1:1)' : '🔵 Preview (2:3)'}
                            </button>
                        ))}
                    </div>

                    <ReactCrop crop={currentCrop} onChange={currentSetter} aspect={aspect}>
                        <img ref={imgRef} src={imageSrc} alt="Crop preview" className="max-w-full h-auto"/>
                    </ReactCrop>

                    <button
                        onClick={handleProcess}
                        disabled={loading}
                        className="bg-green-600 text-white px-4 py-2 rounded hover:bg-green-700 disabled:opacity-50"
                    >
                        {loading ? 'Processing...' : 'Process Image'}
                    </button>
                </>
            )}
        </div>
    );
}
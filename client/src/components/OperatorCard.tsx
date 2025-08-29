import type {Operator} from "../types/operator.ts";

interface Props {
    operator: Operator;
}

export function OperatorCard({operator}: Props) {
    const rarityStars = '★'.repeat(operator.rarity);

    return (
        <div
            className="bg-white rounded-lg shadow-md overflow-hidden hover:shadow-lg transition-shadow w-full max-w-[200px]">
            {/* Изображение на весь размер карточки */}
            <div className="relative w-full h-[300px] bg-gray-200">
                {operator.previewUrl ? (
                    <img
                        src={operator.previewUrl}
                        alt={operator.name}
                        className="w-full h-full object-cover"
                    />
                ) : (
                    <div className="w-full h-full flex items-center justify-center text-gray-400">
                        No image
                    </div>
                )}

                {/* Градиент для лучшей читаемости текста - ВНУТРИ контейнера */}
                <div className="absolute inset-0 bg-gradient-to-t from-black/70 via-transparent to-transparent"></div>

                {/* Текст поверх изображения - ВНУТРИ контейнера */}
                <div className="absolute bottom-0 left-0 right-0 p-3 text-white">
                    <div className="flex justify-between items-end mb-1">
                        <h3 className="text-sm font-bold truncate">{operator.name}</h3>
                        <span className="text-yellow-400 text-xs font-bold ml-1">{rarityStars}</span>
                    </div>

                    <div className="text-xs opacity-90">
                        <p className="truncate">{operator.class}</p>
                        <p className="truncate text-gray-300">{operator.subclass}</p>
                    </div>
                </div>
            </div>
        </div>
    );
}


import React from 'react';
import { Card, Badge } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import { OperatorDto } from '../types';

interface OperatorCardProps {
    operator: OperatorDto;
}

const OperatorCard: React.FC<OperatorCardProps> = ({ operator }) => {
    const getRarityStars = (rarity: number) => {
        return '★'.repeat(rarity);
    };

    const getRarityColor = (rarity: number) => {
        const colors = {
            1: 'secondary',
            2: 'success',
            3: 'info',
            4: 'primary',
            5: 'warning',
            6: 'danger'
        };
        return colors[rarity as keyof typeof colors] || 'secondary';
    };

    return (
        <Link to={`/operators/${operator.id}`} className="text-decoration-none">
            <Card className="operator-card h-100" style={{ cursor: 'pointer' }}>
                <Card.Img
                    variant="top"
                    src={operator.imageUrl}
                    alt={operator.name}
                    style={{ height: '200px', objectFit: 'cover' }}
                    onError={(e) => {
                        (e.target as HTMLImageElement).src = 'https://via.placeholder.com/300x200?text=No+Image';
                    }}
                />
                <Card.Body className="d-flex flex-column">
                    <div className="d-flex justify-content-between align-items-start mb-2">
                        <Card.Title className="mb-0 text-dark">{operator.name}</Card.Title>
                        <Badge bg={getRarityColor(operator.rarity)}>
                            {getRarityStars(operator.rarity)}
                        </Badge>
                    </div>

                    <div className="mb-2">
                        <small className="text-muted">
                            {operator.className} • {operator.subClassName}
                        </small>
                    </div>

                    {operator.factionName && (
                        <div className="mb-2">
                            <Badge bg="outline-secondary" text="dark">
                                {operator.factionName}
                            </Badge>
                        </div>
                    )}

                    <Card.Text className="flex-grow-1 text-muted">
                        {operator.description.length > 100
                            ? `${operator.description.substring(0, 100)}...`
                            : operator.description
                        }
                    </Card.Text>

                    <div className="mt-auto">
                        <small className="text-muted">
                            <i className="bi bi-calendar me-1"></i>
                            {new Date(operator.cnReleaseDate).toLocaleDateString('ru-RU')}
                            {operator.isGlobalReleased && (
                                <Badge bg="success" className="ms-2">Global</Badge>
                            )}
                        </small>
                    </div>
                </Card.Body>
            </Card>
        </Link>
    );
};

export default OperatorCard;

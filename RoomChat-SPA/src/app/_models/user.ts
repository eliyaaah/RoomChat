import { Photo } from './photo';

export interface User {
    id: number;
    username: string;
    displayName: string;
    created: Date;
    lastActive: Date;
    company: string;
    location: string;
    photoUrl?: string;
    description?: string;
    photos?: Photo[];
}

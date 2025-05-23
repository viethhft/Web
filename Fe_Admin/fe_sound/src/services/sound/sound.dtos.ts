export interface SoundDto {
    id: number;
    name: string;
    image: string;
    fileName: string;
    content: Uint8Array;
    contentType: string;
}

export interface AdminSoundDto {
    id: number;
    nameUserAdd: string;
    name: string;
    image: string;
    fileName: string;
    content: string;
    contentType: string;
    dateCreate: Date;
    dateUpdate: Date;
    isDeleted: boolean;
}
export interface AdminSound {
    id: number;
    nameUserAdd: string;
    name: string;
    image: string;
    fileName: string;
    file: File;
    dateCreate: Date;
    dateUpdate: Date;
    isDeleted: boolean;
}

export interface GetMixSoundDto {
    soundName: string;
    image: string;
    fileName: string;
    content: Uint8Array;
    contentType: string;
    idMix: number;
    idSound: number;
}

export interface CreateMixSoundDto {
    name: string;
    idSounds: number[];
}

export interface UpdateMixSoundDto {
    name: string;
    idSounds: number[];
}

export interface AddSound {
    name: string;
    image?: File;
    file?: File;
    token: string;
}

export interface EditSound {
    id: number;
    name: string;
    image?: File;
    file?: File;
}

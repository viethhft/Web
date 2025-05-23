import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../share/Environment/environment';
import { SoundDto, AdminSoundDto, GetMixSoundDto, CreateMixSoundDto, UpdateMixSoundDto, AddSound, EditSound } from './sound.dtos';
import { ResponseData, Pagination, GetList } from '../../share/Dtos/Dtos.Share';
import { toFormBody, toHttpParams } from '../../share/Services/Service.Share';
import { api } from '../../share/Environment/api.link';
@Injectable({
    providedIn: 'root'
})
export class SoundService {
    constructor(private http: HttpClient) { }

    getSound(dataGet: GetList): Observable<ResponseData<Pagination<SoundDto>>> {
        return this.http.get<ResponseData<Pagination<SoundDto>>>(api.sound.getSound, { params: toHttpParams(dataGet) });
    }

    getSoundByAdmin(dataGet: GetList): Observable<ResponseData<Pagination<AdminSoundDto>>> {
        return this.http.get<ResponseData<Pagination<AdminSoundDto>>>(api.sound.getSoundByAdmin, { params: toHttpParams(dataGet) });
    }

    addSound(formData: AddSound): Observable<ResponseData<string>> {
        return this.http.post<ResponseData<string>>(api.sound.addSound, toFormBody(formData));
    }

    updateSound(formData: EditSound): Observable<ResponseData<string>> {
        return this.http.put<ResponseData<string>>(api.sound.updateSound, formData);
    }

    deleteSound(idSound: number): Observable<ResponseData<string>> {
        return this.http.delete<ResponseData<string>>(api.sound.deleteSound, { params: toHttpParams({ id: idSound }) });
    }

    activateSound(idSound: number): Observable<ResponseData<string>> {
        return this.http.patch<ResponseData<string>>(api.sound.activateSound, null, {
            params: toHttpParams({ id: idSound })
        });
    }

    getSoundMix(idMix: number): Observable<ResponseData<GetMixSoundDto[]>> {
        return this.http.get<ResponseData<GetMixSoundDto[]>>(api.sound.getSoundMix, {
            params: toHttpParams({ idMix: idMix })
        });
    }

    createMix(mixData: CreateMixSoundDto): Observable<ResponseData<string>> {
        return this.http.post<ResponseData<string>>(api.sound.createMix, mixData);
    }

    saveMix(updateData: UpdateMixSoundDto): Observable<ResponseData<string>> {
        return this.http.put<ResponseData<string>>(api.sound.saveMix, updateData);
    }
}

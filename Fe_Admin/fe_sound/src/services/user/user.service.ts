import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { GetList, ResponseData, Pagination } from '../../share/Dtos/Dtos.Share';
import {
    CreateUserDto,
    UpdateInfoDto,
    LoginDto,
    CreateAdminDto,
    UpdateRoleUserDto,
    ChangePasswordDto,
    ForgotPasswordDto,
    ActionDto,
    UserDto,
    RoleDto
} from './user.dtos';
import { toHttpParams } from '../../share/Services/Service.Share';

import { api } from '../../share/Environment/api.link';

@Injectable({
    providedIn: 'root'
})
export class UserService {

    constructor(private http: HttpClient) { }

    getListUser(params: GetList): Observable<ResponseData<Pagination<UserDto>>> {
        return this.http.get<ResponseData<Pagination<UserDto>>>(api.user.getListUser, {
            params: toHttpParams(params)
        });
    }

    createUser(user: CreateUserDto): Observable<ResponseData<string>> {
        return this.http.post<ResponseData<string>>(api.user.createUser, user);
    }

    updateUser(updateInfo: UpdateInfoDto): Observable<ResponseData<string>> {
        return this.http.put<ResponseData<string>>(api.user.updateUser, updateInfo);
    }

    deleteUser(action: ActionDto): Observable<ResponseData<string>> {
        return this.http.request<ResponseData<string>>('delete', api.user.deleteUser, { body: action });
    }

    activateUser(action: ActionDto): Observable<ResponseData<string>> {
        return this.http.patch<ResponseData<string>>(api.user.activateUser, action);
    }

    createAdmin(user: CreateAdminDto): Observable<ResponseData<string>> {
        return this.http.post<ResponseData<string>>(api.user.createAdmin, user);
    }

    updateRole(data: UpdateRoleUserDto): Observable<ResponseData<string>> {
        return this.http.put<ResponseData<string>>(api.user.updateRole, data);
    }

    getListRole(): Observable<ResponseData<RoleDto[]>> {
        return this.http.get<ResponseData<RoleDto[]>>(api.user.getListRole);
    }

    changePassword(data: ChangePasswordDto): Observable<ResponseData<string>> {
        return this.http.patch<ResponseData<string>>(api.user.changePassword, data);
    }

    forgotPassword(email: string): Observable<ResponseData<string>> {
        return this.http.get<ResponseData<string>>(api.user.forgotPassword, {
            params: toHttpParams({ email })
        });
    }

    changeForgotPassword(data: ForgotPasswordDto): Observable<ResponseData<string>> {
        return this.http.patch<ResponseData<string>>(api.user.changeForgotPassword, data);
    }
}

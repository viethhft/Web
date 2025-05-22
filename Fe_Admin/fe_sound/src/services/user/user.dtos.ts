export interface CreateUserDto {
    email: string;
    displayName: string;
    gender: boolean;
    token: string;
}

export interface UpdateInfoDto {
    name: string;
    token: string;
    displayName: string;
    phoneNumber: string;
    email: string;
}

export interface LoginDto {
    name?: string | null;
    password?: string | null;
}

export interface CreateAdminDto {
    email: string;
    displayName: string;
    name: string;
    gender: boolean;
    password: string;
}

export interface UpdateRoleUserDto {
    idUser: string;
    listIdRole: string[];
}

export interface ChangePasswordDto {
    token: string;
    oldPassword: string;
    newPassword: string;
    confirmPassword: string;
}

export interface ForgotPasswordDto {
    email: string;
    newPassword: string;
    confirmPassword: string;
    code: string;
}

export interface ActionDto {
    idUser: string;
    token: string;
}

export interface UserDto {
    id: string;
    name: string;
    displayName: string;
    isConfirm: boolean;
    isDeleted: boolean;
    gender: string;
    phoneNumber: string;
    email: string;
    roles: string;
}

export interface RoleDto {
    id: string;
    name: string;
}

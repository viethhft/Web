export interface ResponseData<T> {
    isSuccess: boolean;
    data: T;
    message?: string;
}

export interface Pagination<T> {
    totalPage: number;
    currentPage: number;
    data: T[];
}
export interface GetList {
    PageSize: number;
    PageNumber: number;
}
export interface GetListFilterStatus {
    PageSize: number;
    PageNumber: number;
    Status: boolean
}
export interface GetListSearch {
    PageSize: number;
    PageNumber: number;
    Key: string;
}
export interface GetListFilterRole {
    PageSize: number;
    PageNumber: number;
    Role: string;
}
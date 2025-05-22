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
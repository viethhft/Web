
import { ComponentType } from "@angular/cdk/portal"
import { Injectable } from "@angular/core";
import { MatDialog, MatDialogRef } from "@angular/material/dialog"
export interface DataSettingForm<T = any> {
    width?: string;
    height?: string;
    title?: string;
    data?: T;
}
@Injectable({ providedIn: 'root' })
export class BaseModel {
    TotalPage: number = 0;
    CurrentPage: number = 0;
    PageSize: number = 0;
    IsLoading: boolean = false;

    constructor(private dialog?: MatDialog) {

    }
    showDialog<T, R = any>(
        component: ComponentType<T>,
        config?: DataSettingForm<R>
    ): MatDialogRef<T, R> {
        return this.dialog!.open<T, R>(component, {
            width: config?.width ?? '600px',
            height: config?.height ?? '400px',
            data: config?.data ?? null,
        });
    }

    closeDialog(dialogRef: any) {
        if (dialogRef) {
            dialogRef.close();
        }
    }
}

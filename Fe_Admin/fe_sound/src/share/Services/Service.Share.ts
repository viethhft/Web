import { HttpParams } from '@angular/common/http';

export function toHttpParams(obj: any): HttpParams {
    let params = new HttpParams();

    for (const key of Object.keys(obj)) {
        const value = obj[key];
        if (
            value !== undefined &&
            value !== null &&
            value !== '' &&
            !(typeof value === 'object' && !Array.isArray(value))
        ) {
            // Convert arrays to CSV if needed
            if (Array.isArray(value)) {
                params = params.set(key, value.join(','));
            } else {
                params = params.set(key, value.toString());
            }
        }
    }

    return params;
}


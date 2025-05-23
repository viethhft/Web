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

export function toFormBody(obj: any): FormData {
    const form = new FormData();

    for (const key of Object.keys(obj)) {
        const value = obj[key];

        if (value === undefined || value === null || value === '') {
            // Bỏ qua value null, undefined, empty string
            continue;
        }

        if (Array.isArray(value)) {
            // Nếu là mảng, append từng phần tử với key dạng key[] (tuỳ server xử lý)
            value.forEach((item, index) => {
                // Nếu item là object hay File thì append thẳng, else toString
                if (item instanceof File) {
                    form.append(`${key}[${index}]`, item);
                } else if (typeof item === 'object') {
                    form.append(`${key}[${index}]`, JSON.stringify(item));
                } else {
                    form.append(`${key}[${index}]`, item.toString());
                }
            });
        } else if (value instanceof File) {
            // Nếu là file, append thẳng
            form.append(key, value);
        } else if (typeof value === 'object') {
            // Nếu là object (khác File), stringify JSON
            form.append(key, JSON.stringify(value));
        } else {
            // Giá trị đơn giản (string, number, boolean)
            form.append(key, value.toString());
        }
    }

    return form;
}

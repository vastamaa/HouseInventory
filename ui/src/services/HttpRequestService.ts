import { API_URL } from "../constants";
import { IHttpRequestService } from "./interfaces/IHttpRequestService";

interface IRequestOptions {
    method: string;
    headers: Record<string, string>
    body?: string;
    mode?: 'cors' | 'no-cors' | 'same-origin';
}

interface ISendAsyncProps {
    endpoint: string;
    method: string;
    data?: any;
    customHeaders: Record<string, string>
}
class HttpRequestService implements IHttpRequestService {

    public async GetResourceAsync<T>(endpoint: string, data: any, customHeaders?: Record<string, string>): Promise<T> {
        return await this.SendAsync<T>({ endpoint, method: 'GET', data, customHeaders });
    }

    public async PostResourceAsync<T>(endpoint: string, data: any, customHeaders?: Record<string, string>): Promise<T> {
        return await this.SendAsync<T>({ endpoint, method: 'POST', data, customHeaders });
    }

    public async PutResourceAsync<T>(endpoint: string, data: any, customHeaders?: Record<string, string>): Promise<T> {
        return await this.SendAsync<T>({ endpoint, method: 'PUT', data, customHeaders });
    }

    public async DeleteResourceAsync<T>(endpoint: string, data: any, customHeaders?: Record<string, string>): Promise<T> {
        return await this.SendAsync<T>({ endpoint, method: 'DELETE', data, customHeaders });
    }

    private async SendAsync<T>(props: ISendAsyncProps): Promise<T> {
        const { endpoint, method, data, customHeaders } = props;
        const headers = {
            'Content-Type': 'application/json',
            ...customHeaders
        };

        const options: IRequestOptions = {
            method,
            headers,
            mode: 'cors' // Default mode, can be overridden
        }

        if (data) {
            options.body = JSON.stringify(data); // If data is provided, add it to the body
        }

        try {
            const response = await fetch(endpoint, options);

            if (!response.ok) {
                const errorResponse = await response.json();
                throw new Error(errorResponse.message || 'Something went wrong');
            }
            
            if (response.status === 204) {
                // No content, return null
                return null as any;
            }

            return await response.json();
        }
        catch (error) {
            console.log('HTTP Request failure:', error);
            throw error;
        }
    };
}

export default HttpRequestService;
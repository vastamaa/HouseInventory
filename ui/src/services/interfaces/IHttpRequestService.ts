export interface IHttpRequestService {
    GetResourceAsync<T>(endpoint: string, data: any, customHeaders?: Record<string, string>): Promise<T>;
    PostResourceAsync<T>(endpoint: string, data: any, customHeaders?: Record<string, string>): Promise<T>;
    PutResourceAsync<T>(endpoint: string, data: any, customHeaders?: Record<string, string>): Promise<T>;
    DeleteResourceAsync<T>(endpoint: string, data: any, customHeaders?: Record<string, string>): Promise<T>;
}
import React, { createContext, ReactNode, useContext } from 'react'

// Our stuff
import { IHttpRequestService } from '../services/interfaces/IHttpRequestService';

interface IHttpRequestServiceProviderProps {
    children: ReactNode;
    httpRequestService: IHttpRequestService;
};

const HttpRequestServiceContext = createContext<IHttpRequestService | undefined>(undefined);

export const HttpRequestServiceProvider: React.FC<IHttpRequestServiceProviderProps> = ({ children, httpRequestService }) => {
    return (
        <HttpRequestServiceContext.Provider value={httpRequestService}>
            {children}
        </HttpRequestServiceContext.Provider>
    );
}

export const useHttpRequestService = (): IHttpRequestService => {
    const context = useContext(HttpRequestServiceContext);
    if (context === undefined) {
        throw new Error('useHttpRequestService must be used within a HttpRequestServiceProvider.');
    }
    return context;
}
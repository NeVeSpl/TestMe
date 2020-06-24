import React, {  useEffect } from 'react';
import { ApiError, ApiBaseService } from ".";

export interface IUseRequest
{  
    error: ApiError | undefined;
    isLoading: boolean;
}

export interface IUseRequestWithResult<T> extends IUseRequest
{
    data: T;  
}



export function useRequestWithResult<T>(verb: string, url: string, payload: any, defaultData: T, deps?: ReadonlyArray<unknown>): IUseRequestWithResult<T>
{
    const [isLoading, setIsLoading] = React.useState<boolean>(false);
    const [error, setError] = React.useState<ApiError | undefined>(undefined);
    const [data, setData] = React.useState<T>(defaultData);

    // eslint-disable-next-line react-hooks/exhaustive-deps
    useEffect(() => { ApiBaseService.MakeRequestWithResult<T>(verb, url, payload, setIsLoading, setError, undefined, setData, true); }, deps);

    return { isLoading, error, data } as IUseRequestWithResult<T>;
}

export function useRequest(verb: string, url: string, payload: any, deps?: ReadonlyArray<unknown>): IUseRequest
{
    const [isLoading, setIsLoading] = React.useState(false);
    const [error, setError] = React.useState<ApiError | undefined>(undefined);

    // eslint-disable-next-line react-hooks/exhaustive-deps
    useEffect(() => { ApiBaseService.MakeRequestWithResult<void>(verb, url, payload, setIsLoading, setError, undefined, undefined, false); }, deps);

    return { isLoading, error } as IUseRequest;
}
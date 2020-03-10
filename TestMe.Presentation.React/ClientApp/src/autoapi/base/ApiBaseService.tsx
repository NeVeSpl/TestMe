import { ApiError, ErrorCode, ProblemDetails } from ".";
import { UserService } from "../../services";


export class ApiBaseService 
{
    static readonly API_URL = process.env.REACT_APP_API_URL;

    setError?: (error: ApiError | undefined) => void;
    setLoading?: (isLoading: boolean) => void;
    onConflictError?: (error: ApiError | undefined) => void;

    constructor(setError?: (error: ApiError | undefined) => void, setLoading?: (isLoading: boolean) => void, onConflictError?: (error: ApiError | undefined) => void)
    {
        this.setError = setError;
        this.setLoading = setLoading;
        this.onConflictError = onConflictError;
    }


    public async MakeRequestWithResult<T>(httpMethod: string, url: string, payload: any | undefined = null): Promise<T>
    {
        return ApiBaseService.MakeRequestWithResult<T>(httpMethod, url, payload, this.setLoading, this.setError, this.onConflictError, undefined, true);
    }
    public async MakeRequest(httpMethod: string, url: string, payload: any | undefined = null): Promise<void>
    {
        return ApiBaseService.MakeRequestWithResult<void>(httpMethod, url, payload, this.setLoading, this.setError, this.onConflictError, undefined, false);
    }


    public static async MakeRequestWithResult<T>(httpMethod: string,
        url: string,
        payload?: any,
        setIsLoading?: (isLoading: boolean) => void,
        setError?: (error: ApiError | undefined) => void,
        setConflictError?: (error: ApiError | undefined) => void,
        setData?: (data: T) => void,
        parseContent: boolean = true
    ): Promise<T>        
    {
        setError?.(undefined);
        setIsLoading?.(true);
        const requestInit: RequestInit = ApiBaseService.PrepareRequest(httpMethod, payload);

        try
        {
            let response: Response;
            try
            {
                response = await fetch(`${ApiBaseService.API_URL}/${url}`, requestInit);                
            }
            catch (e)
            {
                const error = new ApiError(ErrorCode.NetworkError);
                throw error;                
            }

            if (!response.ok)
            {
                let problemDetails;
                if (response.headers.has("Content-Type"))
                {
                    const jsonBody = await response.json();
                    console.warn(jsonBody);
                    problemDetails = jsonBody as ProblemDetails;
                }

                const error = new ApiError(response.status, problemDetails);
                throw error;
            }

            if (parseContent)
            {
                if (response.headers.has("Content-Type"))
                {
                    const payload: T = await response.json().then(x => x as T);
                    setData?.(payload);
                    return payload;
                }
                else
                {
                    const error = new ApiError(ErrorCode.NoContentWhenContentWasExpected);
                    throw error;
                }
            }
          
            return {} as T;
        }
        catch (e)
        {
            const error = e as ApiError;
            setError?.(error);
            if (error.errorCode === ErrorCode.Conflict)
            {
                setConflictError?.(error);
            }
            throw error;
        }
        finally
        {
            setIsLoading?.(false);
        }
    }

    public static PrepareRequest(httpMethod: string, payload: any | undefined = null): RequestInit
    {
        const token = UserService.getUserToken();

        let initWithPayload = {};
        let initHeadersWithPayload = {};

        if ((payload !== undefined) && (payload != null))
        {
            initWithPayload = { body: JSON.stringify(payload) };
            initHeadersWithPayload = { "Content-Type": "application/json" };
        }

        const requestInit: RequestInit =
        {
            method: httpMethod,
            mode: 'cors',
            headers:
            {
                "Authorization": token ? 'Bearer ' + token : ""
            }
        };

        Object.assign(requestInit, initWithPayload);
        Object.assign(requestInit.headers, initHeadersWithPayload);

        return requestInit;
    }
}
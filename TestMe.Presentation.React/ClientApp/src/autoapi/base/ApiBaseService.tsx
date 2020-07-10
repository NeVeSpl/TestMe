import { ApiError, ErrorCode, ConflictError, ProblemDetails } from ".";
import { UserService } from "../../services";


export class ApiBaseService 
{
    static readonly API_URL = process.env.REACT_APP_API_URL;

    setError?: (error: ApiError | undefined, url: string) => void;
    setLoading?: (isLoading: boolean, url: string) => void;
    onConflictError?: (error: ApiError | undefined, url: string) => void;

    constructor(setError?: (error: ApiError | undefined, url: string) => void, setLoading?: (isLoading: boolean, url: string) => void, onConflictError?: (error: ApiError | undefined, url: string) => void)
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
        setIsLoading?: (isLoading: boolean, url: string) => void,
        setError?: (error: ApiError | undefined, url : string) => void,
        setConflictError?: (error: ApiError | undefined, url: string) => void,
        setData?: (data: T) => void,
        parseContent: boolean = true
    ): Promise<T>        
    {
        const fullUrl = `${ApiBaseService.API_URL}/${url}`;
        setError?.(undefined, fullUrl);
        setIsLoading?.(true, fullUrl);
        const requestInit: RequestInit = ApiBaseService.PrepareRequest(httpMethod, payload);

        try
        {
            let response: Response;
            try
            {
                response = await fetch(fullUrl, requestInit);                
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
                let error;
                switch (response.status)
                {
                    case ErrorCode.Conflict:
                        error = new ConflictError(response.status, problemDetails as any);
                        break;
                    default:
                        error = new ApiError(response.status, problemDetails);
                        break;
                }
                throw error;
            }

            if (parseContent)
            {
                if (response.headers.has("Content-Type"))
                {
                    const responsePayload: T = await response.json().then(x => x as T);
                    setData?.(responsePayload);
                    return responsePayload;
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
            setError?.(error, fullUrl);
            if (error.errorCode === ErrorCode.Conflict)
            {
                setConflictError?.(error, fullUrl);
            }
            throw error;
        }
        finally
        {
            setIsLoading?.(false, fullUrl);
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
import { ApiError, ErrorCode, ProblemDetails } from ".";


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


    public async MakeRequest<T>(method: string, url: string, payload: any | undefined = null): Promise<T>
    {
        this.InvokeCallbacks(undefined, true);       
        const requestInit: RequestInit = this.PrepareRequest(method, payload);
        let response: Response;

        try
        {
            response = await fetch(`${ApiBaseService.API_URL}/${url}`, requestInit);    
            //await new Promise(resolve => setTimeout(resolve, 500));
        }
        catch (e)
        {
            const error = new ApiError(ErrorCode.NetworkError);
            this.InvokeCallbacks(error, false);
            throw error;            
        }

        return this.ParseResponse<T>(response);
    }

    private PrepareRequest(httpMethod: string, payload: any | undefined = null): RequestInit
    {
        const token = localStorage.getItem("token");

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

    private async ParseResponse<T>(response: Response): Promise<T>
    {
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
            this.InvokeCallbacks(error, false);
            throw error;         
        }

        if (response.headers.has("Content-Type"))
        {
            const payload: T = await response.json().then(x => x as T);
            this.InvokeCallbacks(undefined, false);
            return payload;
        }

        this.InvokeCallbacks(undefined, false);
        return {} as T;
    }

    private InvokeCallbacks(error: ApiError | undefined, isLoading: boolean)
    {
        if ((this.setError !== undefined) && (this.setError !== null))
        {
            this.setError(error);
        }
        if ((this.setLoading !== undefined) && (this.setLoading !== null))
        {
            this.setLoading(isLoading);
        }
        if ((this.onConflictError !== undefined)
            && (this.onConflictError !== null)
            && (error !== undefined)
            && (error.errorCode === ErrorCode.Conflict)
        )
        {
            this.onConflictError(error);
        }
    }    
}
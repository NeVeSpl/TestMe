import { ApiError } from "./services/QuestionsCatalogsService";
import { ThunkDispatch } from "redux-thunk";
import { RootState } from "../redux.base";
import { AnyAction, Action } from 'redux';

type serviceConstructorType<T> = new (setError?: (error: ApiError | undefined, url: string) => void, setLoading?: (isLoading: boolean, url: string) => void, onConflictError?: (error: ApiError | undefined, url: string) => void) => T;

export class ReduxApiFactory
{
    mocks = new Map<string, any>();

    CreateService<T>(typeConstructor: serviceConstructorType<T>, dispatch: ThunkDispatch<RootState, ReduxApiFactory, AnyAction>, invocationOrgin: string): T
    {
        const type = (typeConstructor as any).Type;        

        if (this.mocks.has(type))
        { 
            return this.mocks.get(type) as T;
        }

        return new typeConstructor(
            (x, url) =>
            {
                if (x !== undefined)
                {
                    dispatch(new FetchingErrorOccured(type, invocationOrgin, url, x))
                }
            },
            (x, url) => dispatch(x === true ? new FetchingStarted(type, invocationOrgin, url) : new FetchingEnded(type, invocationOrgin, url)),
            (x, url) => dispatch(new FetchingConflictErrorOccured(type, invocationOrgin, url, x))
        );
    }


    AddMock(type: string, mock : any)
    { 
        this.mocks.set(type, mock);
    }
}

class ApiEvent
{
    constructor(public apiServiceName: string, public invocationOrgin: string, public targetUrl : string) { }
}

export class FetchingStarted extends ApiEvent
{
    static Type = "FetchingStarted";

    constructor(apiServiceName: string, invocationOrgin: string, targetUrl : string, public type = FetchingStarted.Type)
    {
        super(apiServiceName, invocationOrgin, targetUrl);
    }
}
export class FetchingEnded extends ApiEvent
{
    static Type = "FetchingEnded";

    constructor(apiServiceName: string, invocationOrgin: string, targetUrl: string, public type = FetchingEnded.Type)
    {
        super(apiServiceName, invocationOrgin, targetUrl);
    }
}
export class FetchingErrorOccured extends ApiEvent
{   
    static Type = "FetchingErrorOccured"; 

    constructor(apiServiceName: string, invocationOrgin: string, targetUrl: string, public apiError?: ApiError, public type = FetchingErrorOccured.Type)
    {
        super(apiServiceName, invocationOrgin, targetUrl);
    }
}
export class FetchingConflictErrorOccured extends ApiEvent
{
    static Type = "FetchingConflictErrorOccured";

    constructor(apiServiceName: string, invocationOrgin: string, targetUrl: string, public apiError?: ApiError, public type = FetchingConflictErrorOccured.Type)
    {
        super(apiServiceName, invocationOrgin, targetUrl);
    }
}


export class ApiServiceState
{
    isBusy: boolean;
    apiError: ApiError | undefined;    

    constructor()
    {
        this.isBusy = false;
        this.apiError = undefined;
    }
}

export function apiServiceStateReducer(state = new ApiServiceState(), action: Action, invocationOrgin: string, apiServiceName: string): ApiServiceState
{
    switch (action.type)
    {
        case FetchingErrorOccured.Type:
            const errorOccured = action as FetchingErrorOccured;
            if ((errorOccured.invocationOrgin === invocationOrgin) && (errorOccured.apiServiceName === apiServiceName))
            {
                state = { ...state, apiError: errorOccured.apiError };
            }
            break;
        case FetchingStarted.Type:
            const fetchingStarted = action as FetchingStarted;
            if ((fetchingStarted.invocationOrgin === invocationOrgin) && (fetchingStarted.apiServiceName === apiServiceName))
            {
                state = { ...state, isBusy: true, apiError: undefined };
            }
            break;
        case FetchingEnded.Type:
            const fetchingEnded = action as FetchingEnded;
            if ((fetchingEnded.invocationOrgin === invocationOrgin) && (fetchingEnded.apiServiceName === apiServiceName))
            {
                state = { ...state, isBusy: false };
            }
            break;
    }
    return state;
}
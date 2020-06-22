import { ApiError } from "./services/QuestionsCatalogsService";
import { ThunkDispatch } from "redux-thunk";
import { RootState } from "../redux.base";
import { AnyAction, Action } from 'redux';

type serviceConstructorType<T> = new (setError?: (error: ApiError | undefined) => void, setLoading?: (isLoading: boolean) => void, onConflictError?: (error: ApiError | undefined) => void) => T;

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
            (x) =>
            {
                if (x !== undefined)
                {
                    dispatch(new FetchingErrorOccured(type, invocationOrgin, x))
                }
            },
            (x) => dispatch(x === true ? new FetchingStarted(type, invocationOrgin) : new FetchingEnded(type, invocationOrgin)),
            (x) => dispatch(new FetchingConflictErrorOccured(type, invocationOrgin, x))
        );
    }


    AddMock(type: string, mock : any)
    { 
        this.mocks.set(type, mock);
    }
}

export class FetchingStarted
{
    static Type = "FetchingStarted";

    constructor(public apiServiceName: string, public invocationOrgin: string, public type = FetchingStarted.Type) { }
}
export class FetchingEnded
{
    static Type = "FetchingEnded";

    constructor(public apiServiceName: string, public invocationOrgin: string, public type = FetchingEnded.Type) { }
}
export class FetchingErrorOccured
{   
    static Type = "FetchingErrorOccured"; 

    constructor(public apiServiceName: string, public invocationOrgin: string, public apiError?: ApiError, public type = FetchingErrorOccured.Type) { }
}
export class FetchingConflictErrorOccured
{
    static Type = "FetchingConflictErrorOccured";

    constructor(public apiServiceName: string, public invocationOrgin: string, public apiError?: ApiError, public type = FetchingConflictErrorOccured.Type) { }
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
import { QuestionsCatalogsService, ApiError } from "./services/QuestionsCatalogsService";
import { ThunkDispatch } from "redux-thunk";
import { RootState } from "../redux.base";
import { AnyAction } from 'redux';

type serviceConstructorType<T> = new (setError?: (error: ApiError | undefined) => void, setLoading?: (isLoading: boolean) => void, onConflictError?: (error: ApiError | undefined) => void) => T;

export class ReduxApiFactory
{
    mocks = new Map<string, any>();

    CreateService<T>(typeConstructor: serviceConstructorType<T>, dispatch: ThunkDispatch<RootState, ReduxApiFactory, AnyAction>): T
    {
        const type = (typeConstructor as any).Type;        

        if (this.mocks.has(type))
        { 
            return this.mocks.get(type) as T;
        }

        return new typeConstructor(
            (x) => dispatch(new ErrorOccured(type, x)),
            (x) => dispatch(new FetchingData(type, x)),
            (x) => dispatch(new ConflictError(type, x))
        );
    }


    AddMock(type: string, mock : any)
    { 
        this.mocks.set(type, mock);
    }
}


export class ErrorOccured
{   
    static Type = "ErrorOccured"; 

    constructor(public where: string, public apiError?: ApiError, public type = ErrorOccured.Type) { }
}

export class FetchingData
{   
    static Type = "FetchingData";

    constructor(public where: string, public isBusy: boolean, public type = FetchingData.Type) { }
}

export class ConflictError
{
    static Type = "ConflictError";

    constructor(public where: string, public apiError?: ApiError, public type = ConflictError.Type) { }
}
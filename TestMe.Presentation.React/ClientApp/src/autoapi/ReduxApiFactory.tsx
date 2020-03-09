import { QuestionsCatalogsService, ApiError } from "./services/QuestionsCatalogsService";
import { ThunkDispatch } from "redux-thunk";
import { RootState } from "../redux.base";
import { AnyAction } from 'redux';

export class ReduxApiFactory
{
    CreateQuestionsCatalogsService(dispatch: ThunkDispatch<RootState, ReduxApiFactory, AnyAction>): QuestionsCatalogsService
    {
        return new QuestionsCatalogsService(
            (x) => dispatch({ type: ErrorOccured, where: "QuestionsCatalogsService", apiError: x} as ErrorOccured),
            (x) => dispatch({ type: FetchingData, where: "QuestionsCatalogsService", isBusy: x } as FetchingData),
            undefined
        );
    }

}


export const ErrorOccured = 'ErrorOccured';
export const FetchingData = 'FetchingData';



export interface ErrorOccured
{   
    type: typeof ErrorOccured;
    where: string;
    apiError?: ApiError;    
}

export interface FetchingData
{   
    type: typeof FetchingData;
    where: string;
    isBusy: boolean;    
}

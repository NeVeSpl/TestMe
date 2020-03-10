import { QuestionsCatalogsService, ApiError } from "./services/QuestionsCatalogsService";
import { ThunkDispatch } from "redux-thunk";
import { RootState } from "../redux.base";
import { AnyAction } from 'redux';

export class ReduxApiFactory
{
    CreateQuestionsCatalogsService(dispatch: ThunkDispatch<RootState, ReduxApiFactory, AnyAction>): QuestionsCatalogsService
    {
        return new QuestionsCatalogsService(
            (x) => dispatch(new ErrorOccured(QuestionsCatalogsService.Type, x)),
            (x) => dispatch(new FetchingData(QuestionsCatalogsService.Type, x)),
            undefined
        );
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
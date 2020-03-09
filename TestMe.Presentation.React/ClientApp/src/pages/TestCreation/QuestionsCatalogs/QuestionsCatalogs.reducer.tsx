import * as React from 'react';
import { UserService } from '../../../services';
import { ApiError, CatalogHeaderDTO } from '../../../autoapi/services/QuestionsCatalogsService';
import { Thunk } from '../../../redux.base';
import { Action, AnyAction  } from 'redux'
import { ErrorOccured, FetchingData } from '../../../autoapi/ReduxApiFactory';


export enum ChildWindows { None, QuestionsCatalogEditor, QuestionsCatalog }

export const ShowQuestionsCatalog = 'ShowQuestionsCatalog';
export const QuestionsCatalogsFetched = 'QuestionsCatalogsFetched';
export const ShowQuestionsCatalogEditor = 'ShowQuestionsCatalogEditor';
export const CloseWindow = 'CloseWindow';

export class QuestionsCatalogsState
{
    questionsCatalogs: CatalogHeaderDTO[];
    isBusy: boolean;
    apiError: ApiError | undefined;
    openedQuestionsCatalogId: number;
    openedChildWindow: ChildWindows;

    constructor()
    {
        this.questionsCatalogs = [];
        this.isBusy = false;
        this.openedQuestionsCatalogId = 0;
        this.openedChildWindow = ChildWindows.None;
    }
}

export function questionsCatalogsReducer(state = new QuestionsCatalogsState(), action: AnyAction): QuestionsCatalogsState
{
    switch (action.type)
    {
        case ErrorOccured:
            const errorOccured = action as ErrorOccured;
            if (errorOccured.where == "QuestionsCatalogsService")
            {
                state = { ...state, apiError: errorOccured.apiError };
            }
            break;
        case FetchingData:
            const fetchingData = action as FetchingData;
            if (fetchingData.where == "QuestionsCatalogsService")
            {
                state = { ...state, isBusy: fetchingData.isBusy };
            }
            break;
        case QuestionsCatalogsFetched:
            const questionsCatalogsFetched = action as QuestionsCatalogsFetched;
            state = { ...state, questionsCatalogs: questionsCatalogsFetched.questionsCatalogs };
            break;
        case ShowQuestionsCatalogEditor:
            state = { ...state, openedChildWindow: ChildWindows.QuestionsCatalogEditor };
            break;
        case ShowQuestionsCatalog:
            const showQuestionsCatalog = action as ShowQuestionsCatalog;
            state = { ...state, openedChildWindow: ChildWindows.QuestionsCatalog, openedQuestionsCatalogId: showQuestionsCatalog.catalogId };
            break;
        case CloseWindow:
            state = { ...state, openedChildWindow: ChildWindows.None };
            break;
    }
    return state;
}






export function fetchCatalogs(): Thunk<void>
{
    return async (dispatch, getState, api) =>
    {
        const service = api.CreateQuestionsCatalogsService(dispatch);
        service.readQuestionsCatalogHeaders(UserService.getUserID(), { limit: 10, offset: 0 })
            .then(x => dispatch({ type: QuestionsCatalogsFetched, questionsCatalogs: x.result } as QuestionsCatalogsFetched));
    };
}



export interface ShowQuestionsCatalog
{
    type: typeof ShowQuestionsCatalog;   
    catalogId: number;
}

export interface QuestionsCatalogsFetched
{
    type: typeof QuestionsCatalogsFetched;   
    questionsCatalogs: CatalogHeaderDTO[];
}

export interface ShowQuestionsCatalogEditor
{
    type: typeof ShowQuestionsCatalogEditor;   
}
export interface CloseWindow
{
    type: typeof CloseWindow;
}
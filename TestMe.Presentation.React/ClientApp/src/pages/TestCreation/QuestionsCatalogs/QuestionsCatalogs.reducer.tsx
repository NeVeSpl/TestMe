import { UserService } from '../../../services';
import { ApiError, CatalogHeaderDTO, QuestionsCatalogsService } from '../../../autoapi/services/QuestionsCatalogsService';
import { Thunk } from '../../../redux.base';
import { Action } from 'redux'
import { ErrorOccured, FetchingData } from '../../../autoapi/ReduxApiFactory';
import { CloseQuestionsCatalogEditorWindow, QuestionsCatalogCreated, QuestionsCatalogUpdated } from './QuestionsCatalogEditor/QuestionsCatalogEditor.reducer';
import { CloseQuestionsCatalogWindow, QuestionsCatalogDeleted } from './QuestionsCatalog/QuestionsCatalog.reducer';
import { ArrayUtils } from '../../../utils';


export enum ChildWindows { None, QuestionsCatalogEditor, QuestionsCatalog }


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

export function questionsCatalogsReducer(state = new QuestionsCatalogsState(), action: Action): QuestionsCatalogsState
{
    switch (action.type)
    {
        case ErrorOccured.Type:
            const errorOccured = action as ErrorOccured;
            if (errorOccured.where == QuestionsCatalogsService.Type)
            {
                state = { ...state, apiError: errorOccured.apiError };
            }
            break;
        case FetchingData.Type:
            const fetchingData = action as FetchingData;
            if (fetchingData.where == QuestionsCatalogsService.Type)
            {
                state = { ...state, isBusy: fetchingData.isBusy };
            }
            break;
        case QuestionsCatalogsFetched.Type:
            const questionsCatalogsFetched = action as QuestionsCatalogsFetched;
            state = { ...state, questionsCatalogs: questionsCatalogsFetched.questionsCatalogs };
            break;
        case ShowQuestionsCatalogEditor.Type:
            state = { ...state, openedChildWindow: ChildWindows.QuestionsCatalogEditor };
            break;
        case CloseQuestionsCatalogEditorWindow.Type:
            if (state.openedChildWindow == ChildWindows.QuestionsCatalogEditor)
            {
                state = { ...state, openedChildWindow: ChildWindows.None };
            }
            break;
        case ShowQuestionsCatalog.Type:
            const showQuestionsCatalog = action as ShowQuestionsCatalog;
            state = { ...state, openedChildWindow: ChildWindows.QuestionsCatalog, openedQuestionsCatalogId: showQuestionsCatalog.catalogId };
            break;
        case CloseQuestionsCatalogWindow.Type:
            state = { ...state, openedChildWindow: ChildWindows.None };
            break;
        case QuestionsCatalogCreated.Type:
            const questionsCatalogCreated = action as QuestionsCatalogCreated;
            state = { ...state, questionsCatalogs: [...state.questionsCatalogs, questionsCatalogCreated.catalog], openedChildWindow: ChildWindows.None };       
            break;
        case QuestionsCatalogUpdated.Type:
            const questionsCatalogUpdated = action as QuestionsCatalogUpdated;
            state = { ...state, questionsCatalogs: ArrayUtils.ReplaceFirst(state.questionsCatalogs, x => x.catalogId === questionsCatalogUpdated.catalog.catalogId, questionsCatalogUpdated.catalog) };
            break;
        case QuestionsCatalogDeleted.Type:
            const questionsCatalogDeleted = action as QuestionsCatalogDeleted;         
            state = { ...state, questionsCatalogs: state.questionsCatalogs.filter(x => x.catalogId !== questionsCatalogDeleted.catalogId), openedChildWindow: ChildWindows.None };       
            break;

    }
    return state;
}

export function fetchCatalogs(): Thunk<void>
{
    return async (dispatch, getState, api) =>
    {
        const service = api.CreateService(QuestionsCatalogsService, dispatch);
        service.readQuestionsCatalogHeaders(UserService.getUserID(), { limit: 10, offset: 0 })
            .then(x => dispatch(new QuestionsCatalogsFetched( x.result)));
    };
}


export class ShowQuestionsCatalog
{
    static Type = 'ShowQuestionsCatalog';    

    constructor(public catalogId: number, public type = ShowQuestionsCatalog.Type) { }
}

export class QuestionsCatalogsFetched
{
    static Type = 'QuestionsCatalogsFetched'; 

    constructor(public questionsCatalogs: CatalogHeaderDTO[], public type = QuestionsCatalogsFetched.Type) { }
}

export class ShowQuestionsCatalogEditor
{
    static Type = 'ShowQuestionsCatalogEditor';   

    constructor(public type = ShowQuestionsCatalogEditor.Type) { }
}
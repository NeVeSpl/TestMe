import { UserService } from '../../../../services';
import { ApiError, CatalogHeaderDTO, QuestionsCatalogsService, CreateCatalogDTO } from '../../../../autoapi/services/QuestionsCatalogsService';
import { Thunk } from '../../../../redux.base';
import { Action } from 'redux'
import { ErrorOccured, FetchingData } from '../../../../autoapi/ReduxApiFactory';
import { CloseWindow, fetchCatalogs, ChildWindows } from '..';


export class QuestionsCatalogEditorState
{
    apiError: ApiError | undefined;
    isBusy: boolean;   
    formData: CreateCatalogDTO;

    constructor()
    {
        this.isBusy = false;       
        this.formData = new CreateCatalogDTO();
    }
}


export function questionsCatalogEditorReducer(state = new QuestionsCatalogEditorState(), action: Action): QuestionsCatalogEditorState
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
        case QuestionsCatalogFetched.Type:
            const questionsCatalogFetched = action as QuestionsCatalogFetched;
            state = { ...state, formData: questionsCatalogFetched.questionsCatalog };
            break;       
        case CloseWindow.Type:
            const closeWindow = action as CloseWindow;
            if (closeWindow.window === ChildWindows.QuestionsCatalogEditor)
            {
                return new QuestionsCatalogEditorState();
            }
            break;
    }
    return state;
}



export function fetchCatalog(catalogId: number | undefined): Thunk<void>
{
    return async (dispatch, getState, api) =>
    {
        const service = api.CreateService(QuestionsCatalogsService, dispatch);       

        if (catalogId !== undefined)
        {
            service.readQuestionsCatalog(catalogId)
                .then(x => dispatch(new QuestionsCatalogFetched(x)));
        }
    };
}

export function submitCatalog(catalogId: number | undefined, data: CreateCatalogDTO): Thunk<void>
{
    return async (dispatch, getState, api) =>
    {
        const service = api.CreateService(QuestionsCatalogsService, dispatch);

        if (catalogId === undefined)
        {
            service.createCatalog(data)
                .then(x => dispatch(new CloseWindow(ChildWindows.QuestionsCatalogEditor)))
                .then(x => dispatch(fetchCatalogs()));
        }
        else
        {
            service.updateCatalog(catalogId, data)
                .then(x => dispatch(new QuestionsCatalogUpdated(catalogId!)));
        }
    };
}


export class QuestionsCatalogFetched
{
    static Type = 'QuestionsCatalogFetched';

    constructor(public questionsCatalog: CreateCatalogDTO, public type = QuestionsCatalogFetched.Type) { }
}


export class QuestionsCatalogUpdated
{
    static Type = 'QuestionsCatalogUpdated';

    constructor(public catalogId: number, public type = QuestionsCatalogUpdated.Type) { }
}

export class QuestionsCatalogCreated
{
    static Type = 'QuestionsCatalogCreated';

    constructor(public catalogId: number, public type = QuestionsCatalogCreated.Type) { }
}
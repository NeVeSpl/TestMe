import { UserService } from '../../../services';
import { CatalogHeaderDTO, QuestionsCatalogsService } from '../../../autoapi/services/QuestionsCatalogsService';
import { Thunk } from '../../../redux.base';
import { Action } from 'redux'
import { FetchingErrorOccured, FetchingStarted, FetchingEnded, ApiServiceState, apiServiceStateReducer } from '../../../autoapi/ReduxApiFactory';
import { CloseQuestionsCatalogEditorWindow, QuestionsCatalogCreated, QuestionsCatalogUpdated, ShowQuestionsCatalogEditor } from '../QuestionsCatalogEditor/QuestionsCatalogEditor.reducer';
import { CloseQuestionsCatalogWindow, QuestionsCatalogDeleted, ShowQuestionsCatalog } from '../QuestionsCatalog/QuestionsCatalog.reducer';
import { ArrayUtils } from '../../../utils';

export class QuestionsCatalogsState
{
    questionsCatalogs: CatalogHeaderDTO[];
    apiServiceState: ApiServiceState;   
    openedChildWindowCounter: number;

    constructor()
    {
        this.questionsCatalogs = [];
        this.apiServiceState = new ApiServiceState();      
        this.openedChildWindowCounter = 0;
    }
}

const ReducerId = "QuestionsCatalogs";

export function questionsCatalogsReducer(state = new QuestionsCatalogsState(), action: Action): QuestionsCatalogsState
{
    switch (action.type)
    {
        case FetchingErrorOccured.Type:           
        case FetchingStarted.Type:   
        case FetchingEnded.Type:
            state = { ...state, apiServiceState: apiServiceStateReducer(state.apiServiceState, action, ReducerId, QuestionsCatalogsService.Type) };            
            break;
        case QuestionsCatalogsFetched.Type:
            const questionsCatalogsFetched = action as QuestionsCatalogsFetched;
            state = { ...state, questionsCatalogs: questionsCatalogsFetched.questionsCatalogs };
            break;
        case ShowQuestionsCatalogEditor.Type:
        case ShowQuestionsCatalog.Type:
            state = { ...state, openedChildWindowCounter: ++state.openedChildWindowCounter };
            break;
        case CloseQuestionsCatalogEditorWindow.Type:
        case CloseQuestionsCatalogWindow.Type:
            state = { ...state, openedChildWindowCounter: --state.openedChildWindowCounter };
            break;
        case QuestionsCatalogCreated.Type:
            const questionsCatalogCreated = action as QuestionsCatalogCreated;
            state = { ...state, questionsCatalogs: [...state.questionsCatalogs, questionsCatalogCreated.catalog] };       
            break;
        case QuestionsCatalogUpdated.Type:
            const questionsCatalogUpdated = action as QuestionsCatalogUpdated;
            state = { ...state, questionsCatalogs: ArrayUtils.ReplaceFirst(state.questionsCatalogs, x => x.catalogId === questionsCatalogUpdated.catalog.catalogId, questionsCatalogUpdated.catalog) };
            break;
        case QuestionsCatalogDeleted.Type:
            const questionsCatalogDeleted = action as QuestionsCatalogDeleted;         
            state = { ...state, questionsCatalogs: state.questionsCatalogs.filter(x => x.catalogId !== questionsCatalogDeleted.catalogId)};       
            break;
    }
    return state;
}

export class ShowQuestionsCatalogs
{
    static Type = 'QuestionsCatalogs.ShowQuestionsCatalogs';

    constructor(public type = ShowQuestionsCatalogs.Type) { }
}

export function fetchCatalogs(): Thunk<void>
{
    return async (dispatch, getState, api) =>
    {
        const service = api.CreateService(QuestionsCatalogsService, dispatch, ReducerId);
        service.readQuestionsCatalogHeaders(UserService.getUserID(), { limit: 10, offset: 0 })
               .then(x => dispatch(new QuestionsCatalogsFetched( x.result)));
    };
}

export class QuestionsCatalogsFetched
{
    static Type = 'QuestionsCatalogs.QuestionsCatalogsFetched';

    constructor(public questionsCatalogs: CatalogHeaderDTO[], public type = QuestionsCatalogsFetched.Type) { }
}
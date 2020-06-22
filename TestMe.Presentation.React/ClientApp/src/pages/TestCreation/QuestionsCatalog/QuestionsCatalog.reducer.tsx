import { Thunk } from '../../../redux.base';
import { Action } from 'redux';
import { QuestionsService, QuestionHeaderDTO } from '../../../autoapi/services/QuestionsService';
import { QuestionsCatalogsService, CatalogDTO } from '../../../autoapi/services/QuestionsCatalogsService';
import { FetchingErrorOccured, FetchingStarted, FetchingEnded, ApiServiceState, apiServiceStateReducer } from '../../../autoapi/ReduxApiFactory';
import { CloseQuestionsCatalogEditorWindow, QuestionsCatalogUpdated, ShowQuestionsCatalogEditor } from '../QuestionsCatalogEditor/QuestionsCatalogEditor.reducer';
import { QuestionDeleted, CloseQuestionWindow, ShowQuestion } from '../Question/Question.reducer';
import { QuestionUpdated, QuestionCreated, CloseQuestionEditorWindow, ShowQuestionEditor } from '../QuestionEditor/QuestionEditor.reducer';
import { ArrayUtils } from '../../../utils';


export class QuestionsCatalogState
{
    isVisible: boolean;
    catalogId?: number;    
    catalog: CatalogDTO;
    questions: QuestionHeaderDTO[];
    catalogsApiServiceState: ApiServiceState;   
    questionsApiServiceState: ApiServiceState;  
    openedChildWindowCounter: number;
    isDeletePromptVisible: boolean;

    constructor() 
    {
        this.catalogId = undefined
        this.isVisible = false;
        this.catalog = new CatalogDTO();
        this.questions = [];
        this.catalogsApiServiceState = new ApiServiceState();
        this.questionsApiServiceState = new ApiServiceState();
        this.openedChildWindowCounter = 0;     
        this.isDeletePromptVisible = false;
    }
}

const ReducerId = "QuestionsCatalog";

export function questionsCatalogReducer(state = new QuestionsCatalogState(), action: Action): QuestionsCatalogState
{
    switch (action.type)
    {
        case ShowQuestionsCatalog.Type:
            const showQuestionsCatalog = action as ShowQuestionsCatalog;
            state = { ...state, isVisible: true, catalogId: showQuestionsCatalog.catalogId };
            break;
        case FetchingErrorOccured.Type:
        case FetchingStarted.Type:
        case FetchingEnded.Type:
            state = { ...state, catalogsApiServiceState: apiServiceStateReducer(state.catalogsApiServiceState, action, ReducerId, QuestionsCatalogsService.Type) };
            state = { ...state, questionsApiServiceState: apiServiceStateReducer(state.questionsApiServiceState, action, ReducerId, QuestionsService.Type) };
            break; 
        case QuestionsCatalogFetched.Type:
            const questionsCatalogFetched = action as QuestionsCatalogFetched;
            state = { ...state, catalog: questionsCatalogFetched.questionsCatalog };
            break;
        case QuestionsFetched.Type:
            const questionsFetched = action as QuestionsFetched;
            state = { ...state, questions: questionsFetched.questions };
            break;
        case ShowQuestion.Type:
        case ShowQuestionEditor.Type:
        case ShowQuestionsCatalogEditor.Type:
            state = { ...state, openedChildWindowCounter: ++state.openedChildWindowCounter };
            break;
        case CloseQuestionWindow.Type:
        case CloseQuestionEditorWindow.Type:
        case CloseQuestionsCatalogEditorWindow.Type:
            state = { ...state, openedChildWindowCounter: --state.openedChildWindowCounter };
            break;
        case CloseQuestionsCatalogWindow.Type:       
            state = new QuestionsCatalogState();
            break;        
        case QuestionsCatalogUpdated.Type:
            const questionsCatalogUpdated = action as QuestionsCatalogUpdated;
            state = { ...state, catalog: questionsCatalogUpdated.catalog};
            break;
        case QuestionCreated.Type:
            const questionCreated = action as QuestionCreated;
            state = { ...state, questions: [...state.questions, questionCreated.question] };
            break;
        case QuestionUpdated.Type:
            const questionUpdated = action as QuestionUpdated;
            state = { ...state, questions: ArrayUtils.ReplaceFirst(state.questions, x => x.questionId === questionUpdated.question.questionId, questionUpdated.question) };
            break;
        case QuestionDeleted.Type:
            const questionDeleted = action as QuestionDeleted;
            state = { ...state, questions: state.questions.filter(x => x.questionId !== questionDeleted.questionId) };
            break; 
        case OpenQuestionsCatalogDeletePrompt.Type:
            state = { ...state, isDeletePromptVisible: true };
            break;
        case CloseQuestionsCatalogDeletePrompt.Type:
            state = { ...state, isDeletePromptVisible: false };
            break;
    }
    return state;
}

export class ShowQuestionsCatalog
{
    static Type = 'QuestionsCatalog.ShowQuestionsCatalog';

    constructor(public catalogId: number, public type = ShowQuestionsCatalog.Type) { }
}
export class CloseQuestionsCatalogWindow
{
    static Type = 'QuestionsCatalog.CloseQuestionsCatalogWindow';

    constructor(public type = CloseQuestionsCatalogWindow.Type) { }
}

export function fetchCatalog(catalogId?: number): Thunk<void>
{
    return (dispatch, getState, api) =>
    {
        if (catalogId !== undefined)
        {
            const service = api.CreateService(QuestionsCatalogsService, dispatch, ReducerId);
            service.readQuestionsCatalog(catalogId).then(x => dispatch(new QuestionsCatalogFetched(x)));
        }
    };
}
export class QuestionsCatalogFetched
{
    static Type = 'QuestionsCatalog.QuestionsCatalogFetched';

    constructor(public questionsCatalog: CatalogDTO, public type = QuestionsCatalogFetched.Type) { }
}

export function fetchQuestions(catalogId?: number): Thunk<void>
{
    return (dispatch, getState, api) =>
    {
        if (catalogId !== undefined)
        {
            const service = api.CreateService(QuestionsService, dispatch, ReducerId);
            service.readQuestionHeaders(catalogId, { limit: 10, offset: 0 }).then(x => dispatch(new QuestionsFetched(x.result)));
        }
    };
}
export class QuestionsFetched
{
    static Type = 'QuestionsCatalog.QuestionsFetched';

    constructor(public questions: QuestionHeaderDTO[], public type = QuestionsFetched.Type) { }
}

export function deleteCatalog(catalogId: number): Thunk<void>
{
    return async (dispatch, getState, api) =>
    {
        const service = api.CreateService(QuestionsCatalogsService, dispatch, ReducerId);
        service.deleteCatalog(catalogId)
               .then(x => dispatch(new QuestionsCatalogDeleted(catalogId)))
               .then(x => dispatch(new CloseQuestionsCatalogWindow()));
    };
}
export class QuestionsCatalogDeleted
{
    static Type = 'QuestionsCatalog.QuestionsCatalogDeleted';

    constructor(public catalogId: number, public type = QuestionsCatalogDeleted.Type) { }
}

export class OpenQuestionsCatalogDeletePrompt
{
    static Type = 'QuestionsCatalog.OpenQuestionsCatalogDeletePrompt';

    constructor(public type = OpenQuestionsCatalogDeletePrompt.Type) { }
}
export class CloseQuestionsCatalogDeletePrompt
{
    static Type = 'QuestionsCatalog.CloseQuestionsCatalogDeletePrompt';

    constructor(public type = CloseQuestionsCatalogDeletePrompt.Type) { }
}
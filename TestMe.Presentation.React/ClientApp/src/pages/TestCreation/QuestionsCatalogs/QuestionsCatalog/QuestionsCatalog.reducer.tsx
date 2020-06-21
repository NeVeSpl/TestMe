import { Thunk } from '../../../../redux.base';
import { Action } from 'redux';
import { QuestionsService, QuestionHeaderDTO } from '../../../../autoapi/services/QuestionsService';
import { QuestionsCatalogsService, CatalogDTO, ApiError } from '../../../../autoapi/services/QuestionsCatalogsService';
import { ErrorOccured, FetchingData } from '../../../../autoapi/ReduxApiFactory';
import { CloseQuestionsCatalogEditorWindow, QuestionsCatalogUpdated } from '../QuestionsCatalogEditor/QuestionsCatalogEditor.reducer';
import { QuestionDeleted, CloseQuestionWindow } from '../Question/Question.reducer';
import { QuestionUpdated, QuestionCreated } from '../QuestionEditor/QuestionEditor.reducer';
import { ArrayUtils } from '../../../../utils';

export enum ChildWindows { None, QuestionsCatalogEditor, QuestionEditor, Question, QuestionsCatalogDeletePrompt }

export class QuestionsCatalogState
{
    catalog: CatalogDTO;
    questions: QuestionHeaderDTO[];
    catalogsApiError: ApiError | undefined;
    questionsApiError: ApiError | undefined;
    catalogsIsBusy: boolean;
    questionsIsBusy: boolean;
    openedQuestionId: number;
    openedChildWindow: ChildWindows;

    constructor() {
        this.catalog = new CatalogDTO();
        this.questions = [];
        this.catalogsIsBusy = false;
        this.questionsIsBusy = false;
        this.openedQuestionId = 0;
        this.openedChildWindow = ChildWindows.None;
    }
}


export function questionsCatalogReducer(state = new QuestionsCatalogState(), action: Action): QuestionsCatalogState
{
    switch (action.type)
    {
        case ErrorOccured.Type:
            const errorOccured = action as ErrorOccured;
            if (errorOccured.where == QuestionsCatalogsService.Type) 
            {
                state = { ...state, catalogsApiError: errorOccured.apiError };
            }
            if (errorOccured.where == QuestionsService.Type) 
            {
                state = { ...state, questionsApiError: errorOccured.apiError };
            }
            break;
        case FetchingData.Type:
            const fetchingData = action as FetchingData;
            if (fetchingData.where == QuestionsCatalogsService.Type)
            {
                state = { ...state, catalogsIsBusy: fetchingData.isBusy };
            }
            if (fetchingData.where == QuestionsService.Type)
            {
                state = { ...state, questionsIsBusy: fetchingData.isBusy };
            }
            break;
        case QuestionsCatalogFetched.Type:
            const questionsCatalogFetched = action as QuestionsCatalogFetched;
            state = { ...state, catalog: questionsCatalogFetched.questionsCatalog };
            break;
        case QuestionsFetched.Type:
            const questionsFetched = action as QuestionsFetched;
            state = { ...state, questions: questionsFetched.questions };
            break;
        case OpenChildWindow.Type:
            const openChildWindow = action as OpenChildWindow;
            state = { ...state, openedChildWindow: openChildWindow.window };
            break; 
        case ShowQuestion.Type:
            const showQuestion = action as ShowQuestion;
            state = { ...state, openedChildWindow: ChildWindows.Question, openedQuestionId: showQuestion.questionId };
            break;
        case CloseWindow.Type:
        case CloseQuestionWindow.Type:
            state = { ...state, openedChildWindow: ChildWindows.None };
            break;
        case CloseQuestionsCatalogEditorWindow.Type:
            if (state.openedChildWindow == ChildWindows.QuestionsCatalogEditor)
            {
                state = { ...state, openedChildWindow: ChildWindows.None };
            }
            break;
        case CloseQuestionsCatalogWindow.Type:
            state = new QuestionsCatalogState();
            break;
        case QuestionsCatalogDeleted.Type:
            state = { ...state, openedChildWindow: ChildWindows.None };
            break;
        case QuestionsCatalogUpdated.Type:
            const questionsCatalogUpdated = action as QuestionsCatalogUpdated;
            state = { ...state, catalog: questionsCatalogUpdated.catalog, openedChildWindow: ChildWindows.None };
            break;
        case QuestionCreated.Type:
            const questionCreated = action as QuestionCreated;
            state = { ...state, questions: [...state.questions, questionCreated.question], openedChildWindow: ChildWindows.None };
            break;
        case QuestionUpdated.Type:
            const questionUpdated = action as QuestionUpdated;
            state = { ...state, questions: ArrayUtils.ReplaceFirst(state.questions, x => x.questionId === questionUpdated.question.questionId, questionUpdated.question) };
            break;
        case QuestionDeleted.Type:
            const questionDeleted = action as QuestionDeleted;
            state = { ...state, questions: state.questions.filter(x => x.questionId !== questionDeleted.questionId), openedChildWindow: ChildWindows.None };
            break;

    }
    return state;
}


export function fetchCatalog(catalogId: number): Thunk<void>
{
    return (dispatch, getState, api) =>
    {
        const service = api.CreateService(QuestionsCatalogsService, dispatch);
        service.readQuestionsCatalog(catalogId)
               .then(x => dispatch(new QuestionsCatalogFetched(x)));
    };
}

export function fetchQuestions(catalogId: number): Thunk<void>
{
    return (dispatch, getState, api) =>
    {
        const service = api.CreateService(QuestionsService, dispatch);
        service.readQuestionHeaders(catalogId, { limit: 10, offset: 0 })
            .then(x => dispatch(new QuestionsFetched(x.result)));
    };
}
export function deleteCatalog(catalogId: number): Thunk<void>
{
    return async (dispatch, getState, api) =>
    {
        const service = api.CreateService(QuestionsCatalogsService, dispatch);
        service.deleteCatalog(catalogId).then(x => dispatch(new QuestionsCatalogDeleted(catalogId)));
    };
}


export class QuestionsCatalogFetched
{
    static Type = 'QuestionsCatalogFetched';

    constructor(public questionsCatalog: CatalogDTO, public type = QuestionsCatalogFetched.Type) { }
}
export class QuestionsFetched
{
    static Type = 'QuestionsFetched';

    constructor(public questions: QuestionHeaderDTO[], public type = QuestionsFetched.Type) { }
}
export class OpenChildWindow
{
    static Type = 'OpenChildWindow';

    constructor(public window: ChildWindows, public type = OpenChildWindow.Type) { }
}
export class ShowQuestion
{
    static Type = 'ShowQuestion';

    constructor(public questionId: number, public type = ShowQuestion.Type) { }
}


export class CloseWindow
{
    static Type = 'CloseWindow2';

    constructor(public window: ChildWindows, public type = CloseWindow.Type) { }
}

export class CloseQuestionsCatalogWindow
{
    static Type = 'CloseQuestionsCatalogWindow';

    constructor(public type = CloseQuestionsCatalogWindow.Type) { }
}

export class QuestionsCatalogDeleted
{
    static Type = 'QuestionsCatalogDeleted';

    constructor(public catalogId: number, public type = QuestionsCatalogDeleted.Type) { }
}
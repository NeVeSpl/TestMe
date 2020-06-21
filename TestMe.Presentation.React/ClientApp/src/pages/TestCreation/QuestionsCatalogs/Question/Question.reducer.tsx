import { Thunk } from '../../../../redux.base';
import { Action } from 'redux';
import { QuestionDTO, ApiError, QuestionsService } from '../../../../autoapi/services/QuestionsService';
import { ErrorOccured, FetchingData } from '../../../../autoapi/ReduxApiFactory';
import { QuestionUpdated } from '../QuestionEditor/QuestionEditor.reducer';


export enum ChildWindows { None, QuestionDeletePrompt, QuestionEditor }

export class QuestionState
{
    question: QuestionDTO;
    isBusy: boolean;
    apiError: ApiError | undefined;
    openedChildWindow: ChildWindows;

    constructor()
    {
        this.question = new QuestionDTO();
        this.isBusy = false;
        this.openedChildWindow = ChildWindows.None;
    }
}

export function questionReducer(state = new QuestionState(), action: Action): QuestionState
{
    switch (action.type)
    {
        case ErrorOccured.Type:
            const errorOccured = action as ErrorOccured;
            if (errorOccured.where == QuestionsService.Type)
            {
                state = { ...state, apiError: errorOccured.apiError };
            }
            break;
        case FetchingData.Type:
            const fetchingData = action as FetchingData;
            if (fetchingData.where == QuestionsService.Type)
            {
                state = { ...state, isBusy: fetchingData.isBusy };
            }
            break;
        case QuestionFetched.Type:
            const questionFetched = action as QuestionFetched;
            state = { ...state, question: questionFetched.question };
            break;
        case OpenChildWindow.Type:
            const openChildWindow = action as OpenChildWindow;
            state = { ...state, openedChildWindow: openChildWindow.window };
            break; 
        case CloseQuestionWindow.Type:
            state = new QuestionState();
            break;
        case QuestionDeleted.Type:
            state = { ...state, openedChildWindow: ChildWindows.None };
            break;
        case QuestionUpdated.Type:
            const questionUpdated = action as QuestionUpdated;
            state = { ...state, question: questionUpdated.question, openedChildWindow: ChildWindows.None };
            break;
    }
    return state;
}

export function fetchQuestion(questionId: number): Thunk<void>
{
    return (dispatch, getState, api) =>
    {
        const service = api.CreateService(QuestionsService, dispatch);
        service.readQuestionWithAnswers(questionId)
            .then(x => dispatch(new QuestionFetched(x)));
    };
}
export class QuestionFetched
{
    static Type = 'QuestionFetched';

    constructor(public question: QuestionDTO, public type = QuestionFetched.Type) { }
}

export function deleteQuestion(questionId: number): Thunk<void>
{
    return async (dispatch, getState, api) =>
    {
        const service = api.CreateService(QuestionsService, dispatch);
        service.deleteQuestionWithAnswers(questionId).then(x => dispatch(new QuestionDeleted(questionId)));
    };
}

export class QuestionDeleted
{
    static Type = 'QuestionDeleted';

    constructor(public questionId: number, public type = QuestionDeleted.Type) { }
}


export class CloseQuestionWindow
{
    static Type = Symbol('CloseQuestionWindow');

    constructor(public type = CloseQuestionWindow.Type) { }
}
export class OpenChildWindow
{
    static Type = Symbol('OpenChildWindow');

    constructor(public window: ChildWindows, public type = OpenChildWindow.Type) { }
}
export class CloseWindow
{
    static Type = Symbol('CloseWindow');

    constructor(public window: ChildWindows, public type = CloseWindow.Type) { }
}
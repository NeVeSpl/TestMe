import { Thunk } from '../../../redux.base';
import { Action } from 'redux';
import { QuestionsService, QuestionWithAnswersDTO } from '../../../autoapi/services/QuestionsService';
import { FetchingErrorOccured, FetchingStarted, FetchingEnded, ApiServiceState, apiServiceStateReducer } from '../../../autoapi/ReduxApiFactory';
import { QuestionUpdated, CloseQuestionEditorWindow, ShowQuestionEditor } from '../QuestionEditor/QuestionEditor.reducer';

export class QuestionState
{
    isVisible: boolean;
    catalogId?: number;
    questionId?: number;
    question: QuestionWithAnswersDTO;
    apiServiceState: ApiServiceState;   
    openedChildWindowCounter: number;
    isDeletePromptVisible: boolean;

    constructor()
    {
        this.isVisible = false;
        this.question = new QuestionWithAnswersDTO();
        this.apiServiceState = new ApiServiceState();
        this.openedChildWindowCounter = 0;
        this.isDeletePromptVisible = false;
    }
}

const ReducerId = "Question";

export function questionReducer(state = new QuestionState(), action: Action): QuestionState
{
    switch (action.type)
    {
        case ShowQuestion.Type:
            const showQuestionsCatalog = action as ShowQuestion;
            state = { ...state, isVisible: true, catalogId: showQuestionsCatalog.catalogId, questionId: showQuestionsCatalog.questionId };
            break;
        case CloseQuestionWindow.Type:      
            state = new QuestionState();
            break;
        case FetchingErrorOccured.Type:
        case FetchingStarted.Type:
        case FetchingEnded.Type:           
            state = { ...state, apiServiceState: apiServiceStateReducer(state.apiServiceState, action, ReducerId, QuestionsService.Type) };
            break; 
        case QuestionFetched.Type:
            const questionFetched = action as QuestionFetched;
            state = { ...state, question: questionFetched.question };
            break;
        case QuestionUpdated.Type:
            const questionUpdated = action as QuestionUpdated;
            state = { ...state, question: questionUpdated.question };
            break;
        case ShowQuestionDeletePrompt.Type:
            state = { ...state, isDeletePromptVisible: true };
            break;
        case CloseQuestionDeletePrompt.Type:
            state = { ...state, isDeletePromptVisible: false };
            break;     
        case ShowQuestionEditor.Type:
            state = { ...state, openedChildWindowCounter: state.openedChildWindowCounter + 1 };
            break;      
        case CloseQuestionEditorWindow.Type:
            state = { ...state, openedChildWindowCounter: state.openedChildWindowCounter - 1 };
            break;       
    }
    return state;
}

export class ShowQuestion
{
    static Type = 'Question.ShowQuestion';

    constructor(public catalogId: number, public questionId: number, public type = ShowQuestion.Type) { }
}
export class CloseQuestionWindow
{
    static Type = 'Question.CloseQuestionWindow';

    constructor(public type = CloseQuestionWindow.Type) { }
}

export function fetchQuestion(questionId?: number): Thunk<void>
{
    return (dispatch, getState, api) =>
    {
        if (questionId != null)
        {
            const service = api.CreateService(QuestionsService, dispatch, ReducerId);
            service.readQuestionWithAnswers(questionId)
                   .then(x => dispatch(new QuestionFetched(x)));
        }
    };
}
export class QuestionFetched
{
    static Type = 'Question.QuestionFetched';

    constructor(public question: QuestionWithAnswersDTO, public type = QuestionFetched.Type) { }
}

export function deleteQuestion(questionId: number): Thunk<void>
{
    return async (dispatch, getState, api) =>
    {
        const service = api.CreateService(QuestionsService, dispatch, ReducerId);
        service.deleteQuestionWithAnswers(questionId)
               .then(x => dispatch(new QuestionDeleted(questionId)))
               .then(x => dispatch(new CloseQuestionWindow()));
    };
}
export class QuestionDeleted
{
    static Type = 'Question.QuestionDeleted';

    constructor(public questionId: number, public type = QuestionDeleted.Type) { }
}

export class ShowQuestionDeletePrompt
{
    static Type = 'Question.ShowQuestionDeletePrompt';

    constructor(public type = ShowQuestionDeletePrompt.Type) { }
}
export class CloseQuestionDeletePrompt
{
    static Type = 'Question.CloseQuestionDeletePrompt';

    constructor(public type = CloseQuestionDeletePrompt.Type) { }
}
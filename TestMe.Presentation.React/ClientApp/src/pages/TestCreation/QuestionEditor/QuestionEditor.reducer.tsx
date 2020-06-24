import { Thunk } from '../../../redux.base';
import { Action } from 'redux';
import { QuestionDTO, UpdateQuestionDTO, QuestionsService, AnswerDTO, ConflictError as ConflictApiError } from '../../../autoapi/services/QuestionsService';
import { MagicDict, MagicFormState, InputHasChanged, magicFormReducer, MagicForm } from '../../../components';
import { FetchingErrorOccured, FetchingConflictErrorOccured, FetchingStarted, FetchingEnded, ApiServiceState, apiServiceStateReducer } from '../../../autoapi/ReduxApiFactory';

export class QuestionEditorState
{
    isVisible: boolean;
    catalogId?: number;
    questionId?: number;  
    apiServiceState: ApiServiceState;   
    form: MagicFormState<UpdateQuestionDTO>;
    questionBeforeEdit?: UpdateQuestionDTO; 


    constructor()
    {
        this.isVisible = false;
        this.apiServiceState = new ApiServiceState();
        this.form = new MagicFormState<UpdateQuestionDTO>(UpdateQuestionDTO);
    }
}

const ReducerId = "QuestionsEditor";

export function questionsEditorReducer(state = new QuestionEditorState(), action: Action): QuestionEditorState
{
    switch (action.type)
    {
        case ShowQuestionEditor.Type:
            const showQuestionEditor = action as ShowQuestionEditor;
            state = { ...state, isVisible: true, catalogId: showQuestionEditor.catalogId, questionId: showQuestionEditor.questionId };
            break;
        case CloseQuestionEditorWindow.Type:        
            state = new QuestionEditorState();
            break;
        case FetchingErrorOccured.Type:
        case FetchingStarted.Type:
        case FetchingEnded.Type:
            state = { ...state, apiServiceState: apiServiceStateReducer(state.apiServiceState, action, ReducerId, QuestionsService.Type) };
            break; 
        case FetchingConflictErrorOccured.Type:
            const conflictError = action as FetchingConflictErrorOccured;
            const conflictApiError = conflictError.apiError as ConflictApiError;

            const userFormData = state.form.data;
            const originalFormData = state.questionBeforeEdit;
            const serverFormData = conflictApiError.body as UpdateQuestionDTO;
            const result = MagicForm.ResolveConflicts(originalFormData!, userFormData, serverFormData);
            result.resolvedFormData.concurrencyToken = serverFormData.concurrencyToken;
            state = { ...state };
            state.form = { ...state.form };
            state.form.data = result.resolvedFormData;
            state.form.conflictErrors = result.conflictErrors;   
            break;
        case QuestionFetched.Type:
            const questionFetched = action as QuestionFetched;
            state = { ...state, questionBeforeEdit: questionFetched.question as any as UpdateQuestionDTO };
            state.form = { ...state.form };
            state.form.data =  questionFetched.question as any as UpdateQuestionDTO;
            break;
        case InputHasChanged.Type:
            const inputHasChanged = action as InputHasChanged;
            if (inputHasChanged.formId === 'Question')
            {
                const newForm = magicFormReducer(state.form, action);
                isFormValid(newForm);
                state = { ...state, form: newForm  };
            }
            break;
        case FormValidated.Type:
            const formValidated = action as FormValidated;
            state = {...state, form: { ...formValidated.form }};
            break;
      
        case AddAnswer.Type:      
            state = { ...state };
            state.form = { ...state.form };
            state.form.data = { ...state.form.data };           
            state.form.data.answers = [...state.form.data.answers, new AnswerDTO()];           
            break;
        case DeleteAnswer.Type:
            const deleteAnswer = action as DeleteAnswer;
            state = { ...state };
            state.form = { ...state.form };
            state.form.data = { ...state.form.data };
            state.form.data.answers = state.form.data.answers.filter((x, i) => i !== deleteAnswer.answerIndex);           
            break;
    }
    return state;
}

function isFormValid(form: MagicFormState<UpdateQuestionDTO>, forceValidation: boolean = false)
{
    const validationErrors: MagicDict = {};
    if (form.isFormItemTouched.includes('content') || forceValidation)  
    {
        if (form.data.content.length < 3)
        {
            validationErrors['content'] = "Content must be longer than 2 characters";
        }
        if (form.data.content.length > 2048)
        {
            validationErrors['content'] = "Content must be shorter than 2048 characters";
        }
    }
    for (let i = 0; i < form.data.answers.length; ++i)
    {
        const key = `answers[${i}].content`;
        if (form.isFormItemTouched.includes(key) || forceValidation)  
        {
            if (form.data.answers[i].content.length < 3)
            {
                validationErrors[key] = "Answer must be longer than 2 characters";
            }
            if (form.data.answers[i].content.length > 2048)
            {
                validationErrors[key] = "Answer must be shorter than 2048 characters";
            }
        }
    }

    const hasValidationErrors = Object.keys(validationErrors).length > 0;
    form.validationErrors = validationErrors;
    form.hasValidationErrors = hasValidationErrors;  
    return !hasValidationErrors;
} 

export class ShowQuestionEditor
{
    static Type = 'QuestionEditor.ShowQuestionEditor';

    constructor(public catalogId: number, public questionId? : number, public type = ShowQuestionEditor.Type) { }
}
export class CloseQuestionEditorWindow
{
    static Type = 'QuestionEditor.CloseQuestionEditorWindow';

    constructor(public type = CloseQuestionEditorWindow.Type) { }
}


export function fetchQuestion(questionId: number | undefined): Thunk<void>
{
    return (dispatch, getState, api) =>
    {
        if (questionId !== undefined)
        {
            const service = api.CreateService(QuestionsService, dispatch, ReducerId);
            service.readQuestionWithAnswers(questionId)
                   .then(x => dispatch(new QuestionFetched(x)));
        }
    };
}
export class QuestionFetched
{
    static Type = 'QuestionEditor.QuestionFetched';

    constructor(public question: QuestionDTO, public type = QuestionFetched.Type) { }
}

export function saveChanges(questionId: number | undefined, data: UpdateQuestionDTO, catalogId: number): Thunk<void>
{
    return async (dispatch, getState, api) =>
    {
        const service = api.CreateService(QuestionsService, dispatch, ReducerId);

        const form = getState().questionEditor.form;
        if (isFormValid(form, true))
        {
            const questionDTO = { ...data, catalogId };
            if (questionId === undefined)
            {
                service.createQuestionWithAnswers(questionDTO)
                       .then(x => service.readQuestionWithAnswers(x)
                       .then(x => dispatch(new QuestionCreated(x))))
                       .then(x => dispatch(new CloseQuestionEditorWindow()));
            }
            else
            {
                service.updateQuestionWithAnswers(questionId, questionDTO)
                       .then(x => service.readQuestionWithAnswers(questionId)
                       .then(x => dispatch(new QuestionUpdated(x))))
                       .then(x => dispatch(new CloseQuestionEditorWindow()));
            }

        }
        else
        {
            dispatch(new FormValidated(form));
        }
    };
}

export class QuestionUpdated
{
    static Type = 'QuestionEditor.QuestionUpdated';

    constructor(public question: QuestionDTO, public type = QuestionUpdated.Type) { }
}
export class QuestionCreated
{
    static Type = 'QuestionEditor.QuestionCreated';

    constructor(public question: QuestionDTO, public type = QuestionCreated.Type) { }
}
export class FormValidated
{
    static Type = 'QuestionEditor.FormValidated';

    constructor(public form: MagicFormState<UpdateQuestionDTO>, public type = FormValidated.Type) { }
}

export class AddAnswer
{
    static Type = 'QuestionEditor.AddAnswer';

    constructor(public type = AddAnswer.Type) { }
}
export class DeleteAnswer
{
    static Type = 'QuestionEditor.DeleteAnswer';

    constructor(public answerIndex: number, public type = DeleteAnswer.Type) { }
}
import { Thunk } from '../../../../redux.base';
import { Action } from 'redux';
import { QuestionDTO, UpdateQuestionDTO, ApiError, QuestionsService, AnswerDTO, ConflictError as ConflictApiError } from '../../../../autoapi/services/QuestionsService';
import { MagicDict, MagicFormState, InputHasChanged, magicFormReducer, MagicForm } from '../../../../components';
import { ErrorOccured, FetchingData, ConflictError } from '../../../../autoapi/ReduxApiFactory';



export class QuestionEditorState
{
    apiError: ApiError | undefined;
    isBusy: boolean;
    form: MagicFormState<UpdateQuestionDTO>;
    questionBeforeEdit?: UpdateQuestionDTO;
    loadedQuestionId: number | null;
   
    

    constructor()
    {
        this.isBusy = false;
        this.form = new MagicFormState<UpdateQuestionDTO>(UpdateQuestionDTO);
        this.loadedQuestionId = null;
      
    }
}

export function questionsEditorReducer(state = new QuestionEditorState(), action: Action): QuestionEditorState
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
        case ConflictError.Type:
            const conflictError = action as ConflictError;
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
            state.form.data =  questionFetched.question as any as UpdateQuestionDTO ;

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
            state = {...state, form: {... formValidated.form}};
            break;
        case CloseQuestionEditorWindow.Type:
        case QuestionCreated.Type:
        case QuestionUpdated.Type:
            state = new QuestionEditorState();
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


export function fetchQuestion(questionId: number | undefined): Thunk<void>
{
    return (dispatch, getState, api) =>
    {
        if (questionId !== undefined)
        {
            const service = api.CreateService(QuestionsService, dispatch);
            service.readQuestionWithAnswers(questionId)
                .then(x => dispatch(new QuestionFetched(x)));
        }
    };
}

export class QuestionFetched
{
    static Type = 'QuestionFetched';

    constructor(public question: QuestionDTO, public type = QuestionFetched.Type) { }
}


export class QuestionUpdated
{
    static Type = 'QuestionUpdated';

    constructor(public question: QuestionDTO, public type = QuestionUpdated.Type) { }
}

export class QuestionCreated
{
    static Type = Symbol('QuestionCreated');

    constructor(public question: QuestionDTO, public type = QuestionCreated.Type) { }
}


export class CloseQuestionEditorWindow
{
    static Type = 'CloseQuestionEditorWindow';

    constructor(public type = CloseQuestionEditorWindow.Type) { }
}

export class AddAnswer
{
    static Type = 'AddAnswer';

    constructor(public type = AddAnswer.Type) { }
}

export class DeleteAnswer
{
    static Type = 'DeleteAnswer';

    constructor(public answerIndex:number, public type = AddAnswer.Type) { }
}

export class FormValidated
{
    static Type = 'FormValidated';

    constructor(public form: MagicFormState<UpdateQuestionDTO>, public type = FormValidated.Type) { }
}


export function saveChanges(questionId: number | undefined, data: UpdateQuestionDTO, catalogId: number): Thunk<void>
{
    return async (dispatch, getState, api) =>
    {
        const service = api.CreateService(QuestionsService, dispatch);

        const form = getState().questionEditor.form;
        if (isFormValid(form, true))
        {
            const questionDTO = { ...data, catalogId };
            if (questionId === undefined)
            {
                service.createQuestionWithAnswers(questionDTO)
                    .then(x => service.readQuestionWithAnswers(x).then(x => dispatch(new QuestionCreated(x))));
            }
            else
            {
                service.updateQuestionWithAnswers(questionId, questionDTO)
                    .then(x => service.readQuestionWithAnswers(questionId).then(x => dispatch(new QuestionUpdated(x))));
            }

        } else
        {
            dispatch(new FormValidated(form));
        }       
    };
}




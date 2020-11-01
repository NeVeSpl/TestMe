import { QuestionsCatalogsService, CreateCatalogDTO,  CatalogDTO } from '../../../autoapi/services/QuestionsCatalogsService';
import { Thunk } from '../../../redux.base';
import { Action } from 'redux'
import { FetchingErrorOccured, FetchingStarted, FetchingEnded, ApiServiceState, apiServiceStateReducer } from '../../../autoapi/ReduxApiFactory';


export class QuestionsCatalogEditorState
{
    isVisible: boolean;
    catalogId?: number;     
    apiServiceState: ApiServiceState; 
    form: CreateCatalogDTO;
   

    constructor()
    {
        this.form= new CreateCatalogDTO();
        this.isVisible = false;
        this.catalogId = undefined;
        this.apiServiceState = new ApiServiceState();        
    }
}

const ReducerId = "QuestionsCatalogEditor";

export function questionsCatalogEditorReducer(state = new QuestionsCatalogEditorState(), action: Action): QuestionsCatalogEditorState
{
    switch (action.type)
    {
        case ShowQuestionsCatalogEditor.Type:
            const showQuestionsCatalogEditor = action as ShowQuestionsCatalogEditor;
            state = { ...state, isVisible: true, catalogId: showQuestionsCatalogEditor.catalogId };
            break;
        case FetchingErrorOccured.Type:
        case FetchingStarted.Type:
        case FetchingEnded.Type:
            state = { ...state, apiServiceState: apiServiceStateReducer(state.apiServiceState, action, ReducerId, QuestionsCatalogsService.Type) };
            break;
        case QuestionsCatalogFetched.Type:
            const questionsCatalogFetched = action as QuestionsCatalogFetched;
            state = { ...state, form: questionsCatalogFetched.questionsCatalog };
            break;       
        case CloseQuestionsCatalogEditorWindow.Type:
            return new QuestionsCatalogEditorState();            
          
    }
    return state;
}

export class ShowQuestionsCatalogEditor
{
    static Type = 'QuestionsCatalogEditor.ShowQuestionsCatalogEditor';

    constructor(public catalogId?: number, public type = ShowQuestionsCatalogEditor.Type) { }
}
export class CloseQuestionsCatalogEditorWindow
{
    static Type = 'QuestionsCatalogEditor.CloseQuestionsCatalogEditorWindow';

    constructor(public type = CloseQuestionsCatalogEditorWindow.Type) { }
}


export function fetchCatalog(catalogId: number | undefined): Thunk<void>
{
    return async (dispatch, getState, api) =>
    {
        const service = api.CreateService(QuestionsCatalogsService, dispatch, ReducerId);       

        if (catalogId !== undefined)
        {
            service.readQuestionsCatalog(catalogId)
                .then(x => dispatch(new QuestionsCatalogFetched(x as {} as CreateCatalogDTO)));
        }
    };
}
export class QuestionsCatalogFetched
{
    static Type = 'QuestionsCatalogEditor.QuestionsCatalogFetched';

    constructor(public questionsCatalog: CreateCatalogDTO, public type = QuestionsCatalogFetched.Type) { }
}

export function submitCatalog(catalogId: number | undefined, data: CreateCatalogDTO): Thunk<void>
{
    return async (dispatch, getState, api) =>
    {
        const service = api.CreateService(QuestionsCatalogsService, dispatch, ReducerId);

        if (catalogId === undefined)
        {
            service.createCatalog(data)
                   .then(x => service.readQuestionsCatalog(x)
                   .then(x => dispatch(new QuestionsCatalogCreated(x))))
                   .then(x => dispatch(new CloseQuestionsCatalogEditorWindow()));
        }
        else
        {
            service.updateCatalog(catalogId, data)
                   .then(x => service.readQuestionsCatalog(catalogId)
                   .then(x => dispatch(new QuestionsCatalogUpdated(x))))
                   .then(x => dispatch(new CloseQuestionsCatalogEditorWindow()));
        }
    };
}
export class QuestionsCatalogUpdated
{
    static Type = 'QuestionsCatalogEditor.QuestionsCatalogUpdated';

    constructor(public catalog: CatalogDTO, public type = QuestionsCatalogUpdated.Type) { }
}
export class QuestionsCatalogCreated
{
    static Type = 'QuestionsCatalogEditor.QuestionsCatalogCreated';

    constructor(public catalog: CatalogDTO, public type = QuestionsCatalogCreated.Type) { }
}
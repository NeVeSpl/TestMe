import { combineReducers, Action, createStore, applyMiddleware, Middleware, MiddlewareAPI, Dispatch } from 'redux'
import thunk, { ThunkAction } from 'redux-thunk'
import { composeWithDevTools } from 'redux-devtools-extension';
import { ReduxApiFactory } from './autoapi/ReduxApiFactory';
import { ObjectUtils, StateStorage } from './utils/'
import { questionsCatalogsReducer, QuestionsCatalogsState } from './pages/TestCreation/QuestionsCatalogs/QuestionsCatalogs.reducer'
import { questionsCatalogEditorReducer, QuestionsCatalogEditorState } from './pages/TestCreation/QuestionsCatalogs/QuestionsCatalogEditor/QuestionsCatalogEditor.reducer';
import { questionsCatalogReducer, QuestionsCatalogState } from './pages/TestCreation/QuestionsCatalogs/QuestionsCatalog/QuestionsCatalog.reducer';



const rootReducer = combineReducers({
    questionsCatalogs: questionsCatalogsReducer, 
    questionsCatalogEditor: questionsCatalogEditorReducer,
    questionsCatalog: questionsCatalogReducer
})

//export type RootState = ReturnType<typeof rootReducer>
export interface RootState
{
    questionsCatalogs: QuestionsCatalogsState,
    questionsCatalogEditor: QuestionsCatalogEditorState,
    questionsCatalog: QuestionsCatalogState,
}


export type Thunk<ReturnType = void> = ThunkAction<
    ReturnType,
    RootState,
    ReduxApiFactory,
    Action<string>
    >

export type ThunkDispatch = {
    <TAction>(action: TAction):
        TAction extends (...args: any[]) => infer TResult ? TResult :
        TAction,
};

function classToPOJOReduxMiddleware()
{
    const middleware: Middleware = ( api: MiddlewareAPI) => (next: Dispatch) => action =>
    {
        if ((typeof action !== 'function') && (!ObjectUtils.isPlainObject(action)))
        {
            action = { ...action };
        }

        return  next(action);
    }

    return middleware;
}



export function configureStore(reduxStateStorage: StateStorage<RootState>, reduxApiFactory : ReduxApiFactory)
{  
    const reduxState = reduxStateStorage.Load() ?? undefined;
    const store = createStore(rootReducer, reduxState, composeWithDevTools(applyMiddleware(classToPOJOReduxMiddleware(), thunk.withExtraArgument(reduxApiFactory))));

    store.subscribe(() => reduxStateStorage.Save(store.getState()));
    return store;
}
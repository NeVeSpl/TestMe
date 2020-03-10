import { combineReducers, Action, AnyAction } from 'redux'
import { ThunkAction } from 'redux-thunk'
import { createStore, applyMiddleware, Middleware, MiddlewareAPI, Dispatch} from 'redux'
import thunk from 'redux-thunk';
import { questionsCatalogsReducer, QuestionsCatalogsState } from './pages/TestCreation/QuestionsCatalogs'
import { ReduxApiFactory } from './autoapi/ReduxApiFactory';
import { composeWithDevTools } from 'redux-devtools-extension';
import { ObjectUtils, StateStorage } from './utils/'


const rootReducer = combineReducers({
    questionsCatalogs: questionsCatalogsReducer,    
})

//export type RootState = ReturnType<typeof rootReducer>
export interface RootState
{
    questionsCatalogs: QuestionsCatalogsState
}


export type Thunk<ReturnType = void> = ThunkAction<
    ReturnType,
    RootState,
    ReduxApiFactory,
    Action<string>
    >


function classToPOJOReduxMiddleware()
{
    const loggerMiddleware: Middleware = ( api: MiddlewareAPI) => (next: Dispatch) => action =>
    {
        if ((typeof action !== 'function') && (!ObjectUtils.isPlainObject(action)))
        {
            action = { ...action };
        }

        return  next(action);
    }

    return loggerMiddleware;
}



export function configureStore(reduxStateStorage: StateStorage<RootState>, reduxApiFactory : ReduxApiFactory)
{  
    const reduxState = reduxStateStorage.Load();
    const store = createStore(rootReducer, reduxState ?? undefined, composeWithDevTools(applyMiddleware(classToPOJOReduxMiddleware(), thunk.withExtraArgument(reduxApiFactory))));

    store.subscribe(() => reduxStateStorage.Save(store.getState()));
    return store;
}
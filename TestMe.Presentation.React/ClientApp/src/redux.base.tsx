import { combineReducers, Action, AnyAction } from 'redux'
import { ThunkAction } from 'redux-thunk'
import { createStore, applyMiddleware, Middleware, MiddlewareAPI, Dispatch} from 'redux'
import thunk from 'redux-thunk';
import { questionsCatalogsReducer, QuestionsCatalogsState } from './pages/TestCreation/QuestionsCatalogs'
import { ReduxApiFactory } from './autoapi/ReduxApiFactory';
import { composeWithDevTools } from 'redux-devtools-extension';
import { ObjectUtils } from './utils/'


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


export function configureStore()
{
    return createStore(rootReducer, undefined, composeWithDevTools(applyMiddleware(classToPOJOReduxMiddleware(), thunk.withExtraArgument(new ReduxApiFactory))));
}
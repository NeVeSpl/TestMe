import { combineReducers, Action } from 'redux'
import { ThunkAction } from 'redux-thunk'
import { createStore, applyMiddleware } from 'redux'
import thunk from 'redux-thunk';
import { questionsCatalogsReducer, QuestionsCatalogsState } from './pages/TestCreation/QuestionsCatalogs'
import { ReduxApiFactory } from './autoapi/ReduxApiFactory';
import { composeWithDevTools } from 'redux-devtools-extension';


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

export function configureStore()
{
    return createStore(rootReducer, undefined, composeWithDevTools(applyMiddleware(thunk.withExtraArgument(new ReduxApiFactory))));
}




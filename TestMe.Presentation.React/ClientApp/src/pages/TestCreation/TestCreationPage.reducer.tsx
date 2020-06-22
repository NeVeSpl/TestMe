import { Thunk } from '../../redux.base';
import { Action } from 'redux';
import { ShowQuestionsCatalogs } from './QuestionsCatalogs/QuestionsCatalogs.reducer';
import { ShowQuestionsCatalog, CloseQuestionsCatalogWindow } from './QuestionsCatalog/QuestionsCatalog.reducer';
import { ShowQuestionsCatalogEditor, CloseQuestionsCatalogEditorWindow } from './QuestionsCatalogEditor/QuestionsCatalogEditor.reducer';
import { ShowQuestion, CloseQuestionWindow } from './Question/Question.reducer';
import { ShowQuestionEditor, CloseQuestionEditorWindow } from './QuestionEditor/QuestionEditor.reducer';


export class TestCreationPageState
{
    openedWindowStack: string[];


    constructor()
    {
        this.openedWindowStack = [ShowQuestionsCatalogs.Type];
    }
}


export function testCreationPageReducer(state = new TestCreationPageState(), action: Action): TestCreationPageState
{
    switch (action.type)
    {
        case ShowQuestionsCatalog.Type:
        case ShowQuestionsCatalogEditor.Type:
        case ShowQuestion.Type:
        case ShowQuestionEditor.Type:
            state = { ...state, openedWindowStack: [...state.openedWindowStack, action.type]  }
            break;
        case CloseQuestionsCatalogWindow.Type:
        case CloseQuestionsCatalogEditorWindow.Type:
        case CloseQuestionWindow.Type:
        case CloseQuestionEditorWindow.Type:
            state = { ...state, openedWindowStack: state.openedWindowStack.slice(0, -1) }
            break;
    }

    return state;
}
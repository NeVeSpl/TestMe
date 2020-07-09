import * as React from 'react';
import deepFreeze from 'deep-freeze';
import { testCreationPageReducer, TestCreationPageState } from './TestCreationPage.reducer'
import { ShowQuestionsCatalog, CloseQuestionsCatalogWindow } from './QuestionsCatalog/QuestionsCatalog.reducer';
import { ShowQuestionsCatalogEditor, CloseQuestionsCatalogEditorWindow } from './QuestionsCatalogEditor/QuestionsCatalogEditor.reducer';
import { ShowQuestion, CloseQuestionWindow } from './Question/Question.reducer';
import { ShowQuestionEditor, CloseQuestionEditorWindow } from './QuestionEditor/QuestionEditor.reducer';





test('action does not mutate state of TestCreationPage', () =>
{
    const previousState = new TestCreationPageState();

    const actions = [
        new ShowQuestionsCatalog(1),
        new ShowQuestionsCatalogEditor(1),
        new ShowQuestion(1, 1),
        new ShowQuestionEditor(1),
        new CloseQuestionsCatalogWindow(),
        new CloseQuestionsCatalogEditorWindow(),
        new CloseQuestionWindow(),
        new CloseQuestionEditorWindow()
    ];

    deepFreeze(previousState);
    actions.forEach(action => testCreationPageReducer(previousState, action));
});




test('register opening new window on the stack', () =>
{
    const previousState = new TestCreationPageState();
    const expectedNewState = new TestCreationPageState();
    expectedNewState.openedWindowStack.push("QuestionsCatalog.ShowQuestionsCatalog");

    const action = new ShowQuestionsCatalog(1);

    deepFreeze(previousState);
    const newState = testCreationPageReducer(previousState, action);

    expect(newState).toEqual(expectedNewState);
});

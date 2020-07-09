import * as React from 'react';
import deepFreeze from 'deep-freeze'
import { questionsCatalogEditorReducer, QuestionsCatalogEditorState, ShowQuestionsCatalogEditor, QuestionsCatalogFetched, CloseQuestionsCatalogEditorWindow, } from './QuestionsCatalogEditor.reducer'
import { QuestionDTO } from '../../../autoapi/services/QuestionsService';
import { CreateCatalogDTO } from '../../../autoapi/services/QuestionsCatalogsService';




test('action does not mutate state of QuestionsCatalogEditor', () =>
{
    const previousState = new QuestionsCatalogEditorState();

    const actions = [
        new ShowQuestionsCatalogEditor(1),
        new QuestionsCatalogFetched(new CreateCatalogDTO()),
        new CloseQuestionsCatalogEditorWindow(),  
    ];

    deepFreeze(previousState);
    actions.forEach(action => questionsCatalogEditorReducer(previousState, action));
});

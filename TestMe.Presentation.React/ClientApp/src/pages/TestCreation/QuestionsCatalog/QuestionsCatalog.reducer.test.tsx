import * as React from 'react';
import deepFreeze from 'deep-freeze'
import { questionsCatalogReducer, QuestionsCatalogState, ShowQuestionsCatalog, QuestionsCatalogFetched, QuestionsFetched, CloseQuestionsCatalogWindow, OpenQuestionsCatalogDeletePrompt, CloseQuestionsCatalogDeletePrompt } from './QuestionsCatalog.reducer'
import { CatalogOnListDTO, CatalogDTO } from '../../../autoapi/services/QuestionsCatalogsService';
import { QuestionOnListDTO, QuestionWithAnswersDTO } from '../../../autoapi/services/QuestionsService';
import { ShowQuestion, CloseQuestionWindow, QuestionDeleted } from '../Question/Question.reducer';
import { ShowQuestionEditor, CloseQuestionEditorWindow, QuestionCreated, QuestionUpdated } from '../QuestionEditor/QuestionEditor.reducer';
import { ShowQuestionsCatalogEditor, CloseQuestionsCatalogEditorWindow, QuestionsCatalogUpdated } from '../QuestionsCatalogEditor/QuestionsCatalogEditor.reducer';





test('action does not mutate state of QuestionsCatalog', () =>
{
    const previousState = new QuestionsCatalogState();  

    const actions = [
        new ShowQuestionsCatalog(1),
        new QuestionsCatalogFetched(new CatalogDTO()),
        new QuestionsFetched({} as QuestionOnListDTO[]),
        new ShowQuestion(1,1),       
        new ShowQuestionEditor(1, 1),
        new ShowQuestionsCatalogEditor(1),  
        new CloseQuestionWindow(),
        new CloseQuestionEditorWindow(),
        new CloseQuestionsCatalogEditorWindow(),
        new CloseQuestionsCatalogWindow(),
        new QuestionsCatalogUpdated(new CatalogDTO()),
        new QuestionCreated(new QuestionWithAnswersDTO()),
        new QuestionUpdated(new QuestionWithAnswersDTO()),
        new QuestionDeleted(1),
        new OpenQuestionsCatalogDeletePrompt(),
        new CloseQuestionsCatalogDeletePrompt(),
    ];

    deepFreeze(previousState);
    actions.forEach(action => questionsCatalogReducer(previousState, action));   
});

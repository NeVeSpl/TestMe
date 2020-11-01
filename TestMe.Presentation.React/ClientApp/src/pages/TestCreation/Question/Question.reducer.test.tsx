import * as React from 'react';
import deepFreeze from 'deep-freeze'
import { questionReducer, QuestionState, ShowQuestion, CloseQuestionWindow, QuestionFetched, QuestionDeleted, ShowQuestionDeletePrompt, CloseQuestionDeletePrompt } from './Question.reducer'
import { QuestionOnListDTO, QuestionWithAnswersDTO } from '../../../autoapi/services/QuestionsService';
import { FetchingErrorOccured } from '../../../autoapi/ReduxApiFactory';
import { QuestionUpdated, ShowQuestionEditor, CloseQuestionEditorWindow } from '../QuestionEditor/QuestionEditor.reducer';



test('action does not mutate state of Question', () =>
{
    const previousState = new QuestionState();  

    const actions = [
        new ShowQuestion(1, 1),
        new CloseQuestionWindow(),
        new QuestionFetched(new QuestionWithAnswersDTO()),
        new QuestionUpdated(new QuestionWithAnswersDTO()),       
        new ShowQuestionDeletePrompt(),
        new CloseQuestionDeletePrompt(),  
        new ShowQuestionEditor(1, 1),
        new CloseQuestionEditorWindow()
    ];

    deepFreeze(previousState);
    actions.forEach(action =>  questionReducer(previousState, action));   
});

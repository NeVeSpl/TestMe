import * as React from 'react';
import deepFreeze from 'deep-freeze'
import { questionsEditorReducer, QuestionEditorState, QuestionFetched, FormValidated, AddAnswer, DeleteAnswer } from './QuestionEditor.reducer'
import { QuestionOnListDTO, QuestionWithAnswersDTO, UpdateQuestionDTO } from '../../../autoapi/services/QuestionsService';
import { ShowQuestionEditor, CloseQuestionEditorWindow } from '../QuestionEditor/QuestionEditor.reducer';



test('action does not mutate state of QuestionsEditor', () =>
{
    const previousState = new QuestionEditorState();  

    const actions = [
        new ShowQuestionEditor(1, 1),
        new CloseQuestionEditorWindow(),
        new QuestionFetched(new QuestionWithAnswersDTO()),
        new FormValidated(new UpdateQuestionDTO()),       
        new AddAnswer(),
        new DeleteAnswer(1),  
       
    ];

    deepFreeze(previousState);
    actions.forEach(action => questionsEditorReducer(previousState, action));   
});

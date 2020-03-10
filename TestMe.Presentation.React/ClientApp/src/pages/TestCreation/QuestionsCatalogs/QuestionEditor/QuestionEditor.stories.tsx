import React from 'react';
import { RouteComponentProps } from 'react-router-dom';
import { action } from '@storybook/addon-actions';
import QuestionEditor, { QuestionEditorState } from './QuestionEditor';
import { QuestionsService, QuestionDTO } from '../../../../autoapi/services/QuestionsService';
import { StateStorage } from '../../../../utils';


export default {
    title: 'QuestionEditor',
    component: QuestionEditor,
    excludeStories: /.*Data$/,
};

export const actionsData = {
    onCancel: action('onCancel'),
    onQuestionCreated: action('onQuestionCreated'),
    onQuestionUpdated: action('onQuestionUpdated'),
};

export const mockData = {
    catalogId: 1,
    questionId: 1,
    windowNestingLevel: 0,
};

export const Default = () => {


    const questionDTO =
        {
            content: "How are you today?",
            answers:
                [
                    { content: "Gooooood" },
                    { content: "Very gooood" },
                    { content: "Just fine", isCorrect : true }
                ]
        } as QuestionDTO;
   

    const service = new QuestionsService();
    service.readQuestionWithAnswers = (x) => Promise.resolve(questionDTO);

    const storage = StateStorage.CreateMock<QuestionEditorState>();
   

    return (
        <QuestionEditor
            {...mockData}           
            {...({} as RouteComponentProps)}
            {...actionsData}
            injectedService={service}
            injectedStorage={storage} />
    )
};

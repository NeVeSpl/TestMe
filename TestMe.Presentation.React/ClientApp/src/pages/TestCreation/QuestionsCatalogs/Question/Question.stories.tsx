import React from 'react';
import { RouteComponentProps } from 'react-router-dom';
import { action } from '@storybook/addon-actions';
import Question, { QuestionState }  from './Question';
import { QuestionsService, QuestionDTO } from '../../../../autoapi/services/QuestionsService';
import { StateStorage } from '../../../../utils';


export default {
    title: 'Question',
    component: Question,
    excludeStories: /.*Data$/,
};

export const actionsData = {
    onCancel: action('onCancel'),
    onQuestionDeleted: action('onQuestionDeleted'),
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

    const storage = StateStorage.CreateMock(QuestionState);
   

    return (
        <Question
            {...mockData}           
            {...({} as RouteComponentProps)}
            {...actionsData}
            injectedService={service}
            injectedStorage={storage} />
    )
};

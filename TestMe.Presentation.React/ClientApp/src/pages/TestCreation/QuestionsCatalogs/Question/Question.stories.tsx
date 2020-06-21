import React from 'react';
import { RouteComponentProps } from 'react-router-dom';
import { action } from '@storybook/addon-actions';
import Question, { QuestionState } from './Question';
import { Question as QuestionRedux } from './Question.redux';
import { QuestionsService, QuestionDTO } from '../../../../autoapi/services/QuestionsService';
import { StateStorage } from '../../../../utils';
import { ReduxApiFactory } from '../../../../autoapi/ReduxApiFactory';
import { configureStore, RootState } from '../../../../redux.base';
import { Provider } from 'react-redux';


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

    const storage = StateStorage.CreateMock<QuestionState>();
   

    return (
        <Question
            {...mockData}           
            {...({} as RouteComponentProps)}
            {...actionsData}
            injectedService={service}
            injectedStorage={storage} />
    )
};


export const ReduxState = () =>
{
    const questionDTO =
        {
            content: "How are you today?",
            answers:
                [
                    { content: "Gooooood" },
                    { content: "Very gooood" },
                    { content: "Just fine", isCorrect: true }
                ]
        } as QuestionDTO;


    const service = new QuestionsService();
    service.readQuestionWithAnswers = (x) => Promise.resolve(questionDTO);

    const api = new ReduxApiFactory();
    api.AddMock(QuestionsService.Type, service);
   
    const store = configureStore(StateStorage.CreateMock<RootState>(), api);

    return (
        <Provider store={store}>
            <QuestionRedux
                {...mockData}
                {...({} as RouteComponentProps)}
            />
        </Provider>
    );
};

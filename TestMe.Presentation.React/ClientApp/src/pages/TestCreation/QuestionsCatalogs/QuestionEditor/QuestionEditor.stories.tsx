import React from 'react';
import { RouteComponentProps } from 'react-router-dom';
import { action } from '@storybook/addon-actions';
import QuestionEditor, { QuestionEditorState } from './QuestionEditor';
import {QuestionEditor as QuestionEditorRedux} from './QuestionEditor.redux';
import { QuestionsService, QuestionDTO } from '../../../../autoapi/services/QuestionsService';
import { StateStorage } from '../../../../utils';
import { Provider } from 'react-redux';
import { configureStore, RootState } from '../../../../redux.base';
import { ReduxApiFactory } from '../../../../autoapi/ReduxApiFactory';


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

export const LocalState = () => {


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
            <QuestionEditorRedux
                {...mockData}
                {...({} as RouteComponentProps)}
            />
        </Provider>
    );
};

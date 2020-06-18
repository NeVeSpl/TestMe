import React from 'react';
import { RouteComponentProps } from 'react-router-dom';
import { action } from '@storybook/addon-actions';
import QuestionsCatalog, { QuestionsCatalogState } from './QuestionsCatalog';
import { QuestionsCatalog as QuestionsCatalogRedux } from './QuestionsCatalog.redux';
import { QuestionsService, QuestionDTO, OffsetPagedResults, QuestionHeaderDTO } from '../../../../autoapi/services/QuestionsService';
import { StateStorage } from '../../../../utils';
import { QuestionsCatalogsService, QuestionsCatalogDTO } from '../../../../autoapi/services/QuestionsCatalogsService';
import { ReduxApiFactory } from '../../../../autoapi/ReduxApiFactory';
import { RootState, configureStore } from '../../../../redux.base';
import { Provider } from 'react-redux';



export default {
    title: 'QuestionsCatalog.',
    component: QuestionsCatalog,
    excludeStories: /.*Data$/,
};

export const actionsData = {
    onCancel: action('onCancel'),
    onCatalogDeleted: action('onCatalogDeleted'),
    onCatalogUpdated: action('onCatalogUpdated'),
};

export const mockData = {
    catalogId: 1,   
    windowNestingLevel: 0,
};

export const LocalState = () => {


    const questionServiceResult =
        {
            result: [
                {
                    questionId: 0,
                    content: "How are you today?",
                   
                },
                {
                    questionId: 1,
                    content: "Where do you live?",
                }
            ]
        } as OffsetPagedResults<QuestionHeaderDTO>;
   

    const questionService = new QuestionsService();
    questionService.readQuestionHeaders = (x) => Promise.resolve(questionServiceResult);

    const catalogServiceResult =
        {
            name : "catalog A"
        } as QuestionsCatalogDTO;

    const catalogService = new QuestionsCatalogsService();
    catalogService.readQuestionsCatalog = (x) => Promise.resolve(catalogServiceResult);

    const storage = StateStorage.CreateMock<QuestionsCatalogState>();
   

    return (
        <QuestionsCatalog
            {...mockData}           
            {...({} as RouteComponentProps)}
            {...actionsData}
            injectedQuestionService={questionService}
            injectedCatalogService={catalogService}
            injectedStorage={storage} />
    )
};



export const ReduxState = () =>
{
    const questionServiceResult =
        {
            result: [
                {
                    questionId: 0,
                    content: "How are you today?",

                },
                {
                    questionId: 1,
                    content: "Where do you live?",
                }
            ]
        } as OffsetPagedResults<QuestionHeaderDTO>;


    const questionService = new QuestionsService();
    questionService.readQuestionHeaders = (x) => Promise.resolve(questionServiceResult);

    const catalogServiceResult =
        {
            name: "catalog A"
        } as QuestionsCatalogDTO;

    const catalogService = new QuestionsCatalogsService();
    catalogService.readQuestionsCatalog = (x) => Promise.resolve(catalogServiceResult);

    const api = new ReduxApiFactory();
    api.AddMock(QuestionsCatalogsService.Type, catalogService);
    api.AddMock(QuestionsService.Type, questionService);
    const store = configureStore(StateStorage.CreateMock<RootState>(), api);

    return (
        <Provider store={store}>
            <QuestionsCatalogRedux
                {...mockData}
                {...({} as RouteComponentProps)}
            />
        </Provider>
    );
};

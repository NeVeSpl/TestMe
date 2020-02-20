import React from 'react';
import { RouteComponentProps } from 'react-router-dom';
import { action } from '@storybook/addon-actions';
import QuestionsCatalog, { QuestionsCatalogState } from './QuestionsCatalog';
import { QuestionsService, QuestionDTO, OffsetPagedResults, QuestionHeaderDTO } from '../../../../autoapi/services/QuestionsService';
import { StateStorage } from '../../../../utils';
import { QuestionsCatalogsService, QuestionsCatalogDTO } from '../../../../autoapi/services/QuestionsCatalogsService';


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

export const Default = () => {


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

    const storage = StateStorage.CreateMock(QuestionsCatalogState);
   

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

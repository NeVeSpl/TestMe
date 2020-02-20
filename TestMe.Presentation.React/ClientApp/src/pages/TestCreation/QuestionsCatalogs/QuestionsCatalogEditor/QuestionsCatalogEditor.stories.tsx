import React from 'react';
import { RouteComponentProps } from 'react-router-dom';
import { action } from '@storybook/addon-actions';
import QuestionsCatalogEditor, { QuestionsCatalogEditorState } from './QuestionsCatalogEditor';
import { QuestionsService, QuestionDTO, OffsetPagedResults, QuestionHeaderDTO } from '../../../../autoapi/services/QuestionsService';
import { StateStorage } from '../../../../utils';
import { QuestionsCatalogsService, QuestionsCatalogDTO } from '../../../../autoapi/services/QuestionsCatalogsService';


export default {
    title: 'QuestionsCatalogEditor.',
    component: QuestionsCatalogEditor,
    excludeStories: /.*Data$/,
};

export const actionsData = {
    onCancel: action('onCancel'),
    onCatalogCreated: action('onCatalogCreated'),
    onCatalogUpdated: action('onCatalogUpdated'),
};

export const mockData = {
    catalogId: 1,   
    windowNestingLevel: 0,
};

export const Default = () => {


    

   

    const catalogServiceResult =
        {
            name : "catalog A"
        } as QuestionsCatalogDTO;

    const catalogService = new QuestionsCatalogsService();
    catalogService.readQuestionsCatalog = (x) => Promise.resolve(catalogServiceResult);

    const storage = StateStorage.CreateMock(QuestionsCatalogEditorState);
   

    return (
        <QuestionsCatalogEditor
            {...mockData}           
            {...({} as RouteComponentProps)}
            {...actionsData}
            
            injectedService={catalogService}
            injectedStorage={storage} />
    )
};

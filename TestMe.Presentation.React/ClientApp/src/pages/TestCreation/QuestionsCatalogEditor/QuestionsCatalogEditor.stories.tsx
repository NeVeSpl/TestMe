import React from 'react';
import { RouteComponentProps } from 'react-router-dom';
import { action } from '@storybook/addon-actions';
import QuestionsCatalogEditor, { QuestionsCatalogEditorState } from './QuestionsCatalogEditor';
import { QuestionsCatalogEditor as QuestionsCatalogEditorRedux } from './QuestionsCatalogEditor.redux';
import { StateStorage } from '../../../utils';
import { QuestionsCatalogsService, CatalogDTO } from '../../../autoapi/services/QuestionsCatalogsService';
import { ReduxApiFactory } from '../../../autoapi/ReduxApiFactory';
import { RootState, configureStore } from '../../../redux.base';
import { Provider } from 'react-redux';
import { ShowQuestionsCatalogEditor } from './QuestionsCatalogEditor.reducer';


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

export const LocalState = () => 
{  
    const catalogServiceResult =
        {
            name : "catalog A"
        } as CatalogDTO;

    const catalogService = new QuestionsCatalogsService();
    catalogService.readQuestionsCatalog = (x) => Promise.resolve(catalogServiceResult);

    const storage = StateStorage.CreateMock<QuestionsCatalogEditorState>();
   

    return (
        <QuestionsCatalogEditor
            {...mockData}           
            {...({} as RouteComponentProps)}
            {...actionsData}
            
            injectedService={catalogService}
            injectedStorage={storage} />
    )
};

export const ReduxState = () =>
{
    const catalogServiceResult =
        {
            name: "catalog A"
        } as CatalogDTO;

    const catalogService = new QuestionsCatalogsService();
    catalogService.readQuestionsCatalog = (x) => Promise.resolve(catalogServiceResult);

    const api = new ReduxApiFactory();
    api.AddMock(QuestionsCatalogsService.Type, catalogService);
    const store = configureStore(StateStorage.CreateMock<RootState>(), api);
    store.dispatch(new ShowQuestionsCatalogEditor(1));


    return (
        <Provider store={store}>
            <QuestionsCatalogEditorRedux
                {...mockData}     
                {...({} as RouteComponentProps)}
            />
        </Provider>
    );
};

import React from 'react';
import { RouteComponentProps } from 'react-router-dom';
import QuestionsCatalogs, { QuestionsCatalogsState } from './QuestionsCatalogs';
import { QuestionsCatalogs as QuestionsCatalogsRedux } from './QuestionsCatalogs.redux';
import { QuestionsCatalogsService, CatalogHeaderDTO, OffsetPagedResults } from '../../../autoapi/services/QuestionsCatalogsService';
import { StateStorage } from '../../../utils';
import { Provider } from 'react-redux';
import { RootState, configureStore } from '../../../redux.base';
import { ReduxApiFactory } from '../../../autoapi/ReduxApiFactory';


export default {
    title: 'QuestionsCatalogs',
    component: QuestionsCatalogs,
    excludeStories: /.*Data$/,
};



export const Default = () =>
{
    var result =
        {
            result:
            [
                {
                    catalogId: 1,
                    name : "Catalog alpha"
                } ,
                {
                    catalogId: 2,
                    name: "Catalog beta"
                } 
            ]
        } as OffsetPagedResults<CatalogHeaderDTO>;


    const service = new QuestionsCatalogsService();
    service.readQuestionsCatalogHeaders = (x, y) => Promise.resolve(result);
    const storage = StateStorage.CreateMock<QuestionsCatalogsState>();

    return (
        <QuestionsCatalogs injectedService={service} injectedStorage={storage} />
    );
};


export const Redux = () =>
{
    var result =
        {
            result:
                [
                    {
                        catalogId: 1,
                        name: "Catalog alpha"
                    },
                    {
                        catalogId: 2,
                        name: "Catalog beta"
                    }
                ]
        } as OffsetPagedResults<CatalogHeaderDTO>;


    const service = new QuestionsCatalogsService();
    service.readQuestionsCatalogHeaders = (x, y) => Promise.resolve(result);
    const api = new ReduxApiFactory();
    api.AddMock(QuestionsCatalogsService.Type,  service);
    const store = configureStore(StateStorage.CreateMock<RootState>(), api);

    return (
        <Provider store={store}>
            <QuestionsCatalogsRedux />
        </Provider>
    );
};


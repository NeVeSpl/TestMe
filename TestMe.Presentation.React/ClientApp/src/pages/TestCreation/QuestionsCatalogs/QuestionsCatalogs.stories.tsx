import React from 'react';
import { RouteComponentProps } from 'react-router-dom';
import QuestionsCatalogs, { QuestionsCatalogsState }  from './QuestionsCatalogs';
import { QuestionsCatalogsService, CatalogHeaderDTO, OffsetPagedResults } from '../../../autoapi/services/QuestionsCatalogsService';
import { StateStorage } from '../../../utils';

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
    const storage = StateStorage.CreateMock(QuestionsCatalogsState);

    return (
        <QuestionsCatalogs injectedService={service} injectedStorage={storage} />
    );
};

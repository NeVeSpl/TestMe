import * as React from 'react';
import deepFreeze from 'deep-freeze'
import { questionsCatalogsReducer, QuestionsCatalogsState, QuestionsCatalogsFetched } from './QuestionsCatalogs.reducer'
import { CreateCatalogDTO, CatalogDTO, CatalogOnListDTO} from '../../../autoapi/services/QuestionsCatalogsService';
import { ShowQuestionsCatalogEditor, CloseQuestionsCatalogEditorWindow, QuestionsCatalogCreated, QuestionsCatalogUpdated } from '../QuestionsCatalogEditor/QuestionsCatalogEditor.reducer';
import { ShowQuestionsCatalog, CloseQuestionsCatalogWindow, QuestionsCatalogDeleted } from '../QuestionsCatalog/QuestionsCatalog.reducer';




test('action does not mutate state of QuestionsCatalogs', () =>
{
    const previousState = new QuestionsCatalogsState();

    const actions = [
        new QuestionsCatalogsFetched([] as CatalogOnListDTO[]),
        new ShowQuestionsCatalogEditor(1),
        new ShowQuestionsCatalog(1),  
        new CloseQuestionsCatalogEditorWindow(),
        new CloseQuestionsCatalogWindow(),
        new QuestionsCatalogCreated(new CatalogDTO()),
        new QuestionsCatalogUpdated(new CatalogDTO()),
        new QuestionsCatalogDeleted(1)
    ];

    deepFreeze(previousState);
    actions.forEach(action => questionsCatalogsReducer(previousState, action));
});

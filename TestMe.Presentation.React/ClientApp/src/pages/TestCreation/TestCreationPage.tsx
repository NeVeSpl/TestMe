import * as React from 'react';
import { RouteComponentProps } from 'react-router-dom';
import { QuestionsCatalogs } from './';
import { QuestionsCatalogs as QuestionsCatalogsRedux } from './QuestionsCatalogs/QuestionsCatalogs.redux';
import { QuestionsCatalog as QuestionsCatalogRedux } from './QuestionsCatalog/QuestionsCatalog.redux';
import { QuestionsCatalogEditor as QuestionsCatalogEditorRedux } from './QuestionsCatalogEditor/QuestionsCatalogEditor.redux';
import { Question as QuestionRedux } from './Question/Question.redux';
import { QuestionEditor as QuestionEditorRedux } from './QuestionEditor/QuestionEditor.redux';
import { RootState } from '../../redux.base';
import { useSelector } from 'react-redux';
import { ShowQuestionsCatalog } from './QuestionsCatalog/QuestionsCatalog.reducer';
import { ShowQuestionsCatalogEditor } from './QuestionsCatalogEditor/QuestionsCatalogEditor.reducer';
import { ShowQuestion } from './Question/Question.reducer';
import { ShowQuestionEditor } from './QuestionEditor/QuestionEditor.reducer';


interface TestCreationPageProps extends RouteComponentProps
{

}

export function TestCreationPage(props : TestCreationPageProps)
{
    const { openedWindowStack } = useSelector((state: RootState) => state.testCreationPage);  
   
    return (
        <div className="container">
            <div className="row">
                <div className="col-md">
                    <h3>Local state management</h3>
                    <div className="position-relative">
                        <QuestionsCatalogs />
                    </div>
                </div>
                <div className="col-md">
                    <h3>Redux state management</h3>
                    <div className="position-relative">
                        <QuestionsCatalogsRedux />
                        <QuestionsCatalogRedux windowNestingLevel={openedWindowStack.indexOf(ShowQuestionsCatalog.Type) } />
                        <QuestionsCatalogEditorRedux windowNestingLevel={openedWindowStack.indexOf(ShowQuestionsCatalogEditor.Type) } />
                        <QuestionRedux windowNestingLevel={openedWindowStack.indexOf(ShowQuestion.Type) } />
                        <QuestionEditorRedux windowNestingLevel={openedWindowStack.indexOf(ShowQuestionEditor.Type) } />
                    </div>
                </div>
            </div>
        </div>
    );
    
}
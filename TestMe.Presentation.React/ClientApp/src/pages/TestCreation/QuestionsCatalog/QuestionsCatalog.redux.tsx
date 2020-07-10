import * as React from 'react';
import { RouteProps } from 'react-router';
import { useDispatch, useSelector } from 'react-redux';
import { useEffect } from 'react';
import { fetchCatalog, fetchQuestions, deleteCatalog, CloseQuestionsCatalogWindow, OpenQuestionsCatalogDeletePrompt, CloseQuestionsCatalogDeletePrompt } from './QuestionsCatalog.reducer';
import { RootState, ThunkDispatch } from '../../../redux.base';
import { BusyIndicator, Window, Prompt } from '../../../components';
import { StringUtils } from '../../../utils';
import { preventDefault } from '../../../utils/ReactUtils';
import { ShowQuestion } from '../Question/Question.reducer';
import { ShowQuestionEditor } from '../QuestionEditor/QuestionEditor.reducer';
import { ShowQuestionsCatalogEditor } from '../QuestionsCatalogEditor/QuestionsCatalogEditor.reducer';


interface QuestionsCatalogProps extends RouteProps
{    
    windowNestingLevel: number;
}

export function QuestionsCatalog(props: QuestionsCatalogProps)
{
    const { catalog, questions, catalogsApiServiceState, questionsApiServiceState, catalogId, isVisible, openedChildWindowCounter, isDeletePromptVisible } = useSelector((state: RootState) => state.questionsCatalog);  
    const dispatch = useDispatch<ThunkDispatch>()
    // eslint-disable-next-line react-hooks/exhaustive-deps
    useEffect(() => { dispatch(fetchCatalog(catalogId)) }, [catalogId]);  
    // eslint-disable-next-line react-hooks/exhaustive-deps
    useEffect(() => { dispatch(fetchQuestions(catalogId)) }, [catalogId]);   
    const handleCancel = () => { dispatch(new CloseQuestionsCatalogWindow()) };

    return (!isVisible ? null :
        <>
            <Window level={props.windowNestingLevel} title={"Catalog : " + catalog.name} onCancel={handleCancel} onOk={handleCancel} error={catalogsApiServiceState.apiError || questionsApiServiceState.apiError} isEnabled={openedChildWindowCounter === 0 && isDeletePromptVisible === false}>
                <div className="text-right mb-3" >
                    <button type="button" className="btn btn-outline-danger mr-1" onClick={preventDefault(() => dispatch(new OpenQuestionsCatalogDeletePrompt()))}>Delete catalog</button>
                    <button type="button" className="btn btn-outline-info m-0" onClick={preventDefault(() => dispatch(new ShowQuestionsCatalogEditor(catalogId)))}>Edit catalog</button>
                </div>
                <BusyIndicator isBusy={questionsApiServiceState.isBusy || catalogsApiServiceState.isBusy}>
                    {() =>
                        <>
                            <div className="list-group">
                                {questions.sort((a, b) => a.content.localeCompare(b.content))
                                    .map(x => <a
                                        className="list-group-item list-group-item-action"
                                        href="#"
                                        key={x.questionId}
                                        onClick={preventDefault(() => dispatch(new ShowQuestion(catalogId!, x.questionId)))}> { StringUtils.Truncate40(x.content) }</a>)}
                                              </div>
                            <div className="mt-3">       
                                <button type="button" className="btn btn-outline-primary" onClick={preventDefault(() => dispatch(new ShowQuestionEditor(catalogId!)))}>Add question</button>
                            </div>
                        </>
                    }
                </BusyIndicator>
            </Window>
            {!isDeletePromptVisible ? null :                    
                            <Prompt
                                level={props.windowNestingLevel + 1}
                                onOk={() => dispatch(deleteCatalog(catalogId!))}
                                message="Are you sure?"
                                onCancel={() => dispatch(new CloseQuestionsCatalogDeletePrompt())}
                            />}            
        </>
    );
}
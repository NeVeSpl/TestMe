import * as React from 'react';
import { RouteProps } from 'react-router';
import { useDispatch, useSelector } from 'react-redux';
import { useEffect } from 'react';
import { QuestionsCatalogEditor } from '../QuestionsCatalogEditor/QuestionsCatalogEditor.redux';
import { Question, QuestionEditor } from '../';
import { fetchCatalog, fetchQuestions, ChildWindows, OpenChildWindow, CloseWindow, ShowQuestion, deleteCatalog, CloseQuestionsCatalogWindow } from './QuestionsCatalog.reducer';
import { RootState, ThunkDispatch } from '../../../../redux.base';
import { BusyIndicator, Window, Prompt } from '../../../../components';
import { StringUtils } from '../../../../utils';
import { preventDefault } from '../../../../utils/ReactUtils';


interface QuestionsCatalogProps extends RouteProps
{
    catalogId: number;  
    windowNestingLevel: number;
}

export function QuestionsCatalog(props: QuestionsCatalogProps)
{
    const { catalog, questions, catalogsApiError, questionsApiError, catalogsIsBusy, questionsIsBusy, openedQuestionId, openedChildWindow } = useSelector((state: RootState) => state.questionsCatalog);  
    const dispatch = useDispatch<ThunkDispatch>()
    useEffect(() => { dispatch(fetchCatalog(props.catalogId)) }, [props.catalogId]);   
    useEffect(() => { dispatch(fetchQuestions(props.catalogId)) }, [props.catalogId]);   
    const handleCancel = () => { dispatch(new CloseQuestionsCatalogWindow()) };

    return (
        <>
            <Window level={props.windowNestingLevel} title={"Catalog : " + catalog.name} onCancel={handleCancel} onOk={handleCancel} error={catalogsApiError || questionsApiError} isEnabled={openedChildWindow === ChildWindows.None}>
                <div className="text-right mb-3" >
                    <button type="button" className="btn btn-outline-danger mr-1" onClick={preventDefault(() => dispatch(new OpenChildWindow(ChildWindows.QuestionsCatalogDeletePrompt)))}>Delete catalog</button>
                    <button type="button" className="btn btn-outline-info m-0" onClick={preventDefault(() => dispatch(new OpenChildWindow(ChildWindows.QuestionsCatalogEditor)))}>Edit catalog</button>
                </div>
                <BusyIndicator isBusy={questionsIsBusy || catalogsIsBusy}>
                    {() =>
                        <div className="list-group">
                            {questions.sort((a, b) => a.content.localeCompare(b.content))
                                .map(x => <a
                                    className="list-group-item list-group-item-action"
                                    href="#"
                                    key={x.questionId}
                                    onClick={preventDefault(() => dispatch(new ShowQuestion(x.questionId)))}> { StringUtils.Truncate40(x.content) }</a>)}
                            <button type="button" className="list-group-item list-group-item-action list-group-item-primary text-center" onClick={preventDefault(() => dispatch(new OpenChildWindow(ChildWindows.QuestionEditor)))}>Add new question</button>
                        </div>
                    }
                </BusyIndicator>
            </Window>
            {(() =>
            {
                switch (openedChildWindow)
                {
                    case ChildWindows.QuestionsCatalogDeletePrompt:
                        return (
                            <Prompt
                                level={props.windowNestingLevel + 1}
                                onOk={() => dispatch(deleteCatalog(props.catalogId))}
                                message="Are you sure?"
                                onCancel={() => dispatch(new CloseWindow(ChildWindows.QuestionsCatalogDeletePrompt))}
                            />
                        );
                    case ChildWindows.Question:
                        return (
                            <Question
                                windowNestingLevel={props.windowNestingLevel + 1}
                                catalogId={props.catalogId}
                                questionId={openedQuestionId}
                                onCancel={() => dispatch(new CloseWindow(ChildWindows.Question))}
                                onQuestionUpdated={() =>
                                {
                                    dispatch(new CloseWindow(ChildWindows.QuestionEditor));
                                    dispatch(fetchQuestions(props.catalogId));
                                }
                                }
                                onQuestionDeleted={() =>
                                {
                                    dispatch(new CloseWindow(ChildWindows.QuestionEditor));
                                    dispatch(fetchQuestions(props.catalogId));
                                }
                                }
                            />
                        );
                    case ChildWindows.QuestionsCatalogEditor:
                        return (
                            <QuestionsCatalogEditor
                                windowNestingLevel={props.windowNestingLevel + 1}
                                catalogId={props.catalogId}
                            />
                        );
                    case ChildWindows.QuestionEditor:
                        return (
                            <QuestionEditor
                                windowNestingLevel={props.windowNestingLevel + 1}
                                catalogId={props.catalogId}
                                onCancel={() => dispatch(new CloseWindow(ChildWindows.QuestionEditor))}
                                onQuestionCreated={() =>
                                {
                                    dispatch(new CloseWindow(ChildWindows.QuestionEditor));
                                    dispatch(fetchQuestions(props.catalogId));
                                }
                                }
                            />
                        );
                }
            })()
            }
        </>
    );
}
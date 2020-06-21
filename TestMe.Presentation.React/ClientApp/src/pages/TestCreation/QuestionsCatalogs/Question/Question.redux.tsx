import * as React from 'react';
import { ThunkDispatch } from 'redux-thunk';
import { useDispatch, useSelector } from 'react-redux';
import { BusyIndicator, Window, Prompt } from '../../../../components';
import { ChildWindows, CloseQuestionWindow, OpenChildWindow, fetchQuestion, deleteQuestion, CloseWindow } from './Question.reducer';
import { RootState } from '../../../../redux.base';
import style from './Question.module.css';
import { preventDefault } from '../../../../utils/ReactUtils';
import { useEffect } from 'react';
import { QuestionEditor } from '../QuestionEditor/QuestionEditor.redux';

interface QuestionProps 
{
    catalogId: number;
    questionId: number; 
    windowNestingLevel: number;
}


export function Question(props: QuestionProps)
{
    const { apiError, isBusy, openedChildWindow, question  } = useSelector((state: RootState) => state.question);
    const dispatch = useDispatch();
    useEffect(() => { dispatch(fetchQuestion(props.questionId)) }, [props.questionId]);   
    const handleCancel = () => { dispatch(new CloseQuestionWindow()) };

    return (
        <>
            <Window level={props.windowNestingLevel} title={"Question : " + question.content} onCancel={handleCancel} onOk={handleCancel} error={apiError} isEnabled={openedChildWindow === ChildWindows.None}>
                <div className="text-right mb-3" >
                    <button type="button" className="btn btn-outline-danger mr-1" onClick={preventDefault(() => dispatch(new OpenChildWindow(ChildWindows.QuestionDeletePrompt)))} >Delete question</button>
                    <button type="button" className="btn btn-outline-info" onClick={ preventDefault(() => dispatch(new OpenChildWindow(ChildWindows.QuestionEditor)))}>Edit question</button>
                </div>
                <BusyIndicator isBusy={isBusy}>
                    {() =>
                        <>
                            <p>{question.content}</p>

                            <ul>{question.answers.map(x =>
                            {
                                const className = x.isCorrect ? style.correct : style.incorrect;
                                return <li className={className} key={x.answerId}>{x.content}</li>;

                            })
                            }
                            </ul>
                        </>
                    }
                </BusyIndicator>
            </Window>
            {(() =>
            {
                switch (openedChildWindow)
                {
                    case ChildWindows.QuestionDeletePrompt:
                        return (
                            <Prompt
                                level={props.windowNestingLevel + 1}
                                onOk={() => { dispatch(deleteQuestion(props.questionId)) }}
                                message="Are you sure?"
                                onCancel={() => dispatch(new CloseWindow(ChildWindows.QuestionDeletePrompt))}
                            />
                        );
                    case ChildWindows.QuestionEditor:
                        return (
                            <QuestionEditor
                                windowNestingLevel={props.windowNestingLevel + 1}
                                catalogId={props.catalogId}
                                questionId={props.questionId}
                               
                            />
                        );
                }
            })()
            }
        </>
    );
}
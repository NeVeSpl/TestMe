import * as React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { BusyIndicator, Window, Prompt } from '../../../components';
import { CloseQuestionWindow,  fetchQuestion, deleteQuestion, CloseQuestionDeletePrompt, ShowQuestionDeletePrompt } from './Question.reducer';
import { RootState } from '../../../redux.base';
import style from './Question.module.css';
import { preventDefault } from '../../../utils/ReactUtils';
import { useEffect } from 'react';
import { ShowQuestionEditor } from '../QuestionEditor/QuestionEditor.reducer';

interface QuestionProps 
{ 
    windowNestingLevel: number;
}

export function Question(props: QuestionProps)
{
    const { apiServiceState, openedChildWindowCounter, isDeletePromptVisible, question, questionId, catalogId, isVisible } = useSelector((state: RootState) => state.question);
    const dispatch = useDispatch();
    // eslint-disable-next-line react-hooks/exhaustive-deps
    useEffect(() => { dispatch(fetchQuestion(questionId)) }, [questionId]);   
    const handleCancel = () => { dispatch(new CloseQuestionWindow()) };

    return (!isVisible ? null :
        <>
            
                <Window level={props.windowNestingLevel} title={"Question : " + question.content} onCancel={handleCancel} onOk={handleCancel} error={apiServiceState.apiError} isEnabled={openedChildWindowCounter === 0 && isDeletePromptVisible === false}>
                    <div className="text-right mb-3" >
                        <button type="button" className="btn btn-outline-danger mr-1" onClick={preventDefault(() => dispatch(new ShowQuestionDeletePrompt()))} >Delete question</button>
                        <button type="button" className="btn btn-outline-info" onClick={preventDefault(() => dispatch(new ShowQuestionEditor(catalogId!, questionId)))}>Edit question</button>
                    </div>
                    <BusyIndicator isBusy={apiServiceState.isBusy}>
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
            {!isDeletePromptVisible ? null :
             
                            <Prompt
                                level={props.windowNestingLevel + 1}
                                onOk={() => { dispatch(deleteQuestion(questionId!)) }}
                                message="Are you sure?"
                                onCancel={() => dispatch(new CloseQuestionDeletePrompt())}
                            />
            }
        </>
    );
}
import * as React from 'react';
import { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { fetchQuestion, CloseQuestionEditorWindow, AddAnswer, DeleteAnswer, saveChanges } from './QuestionEditor.reducer';
import { Window, MagicForm, MagicDict, BusyIndicator, MagicTextInput } from '../../../components';
import { RootState } from '../../../redux.base';
import { ReactComponent as DeleteIco } from '../outline-delete_forever-24px.svg';
import { InputHasChanged } from '../../../components/MagicForm/MagicFormReducer';
import { preventDefault } from '../../../utils/ReactUtils';

interface QuestionEditorProps
{  
    windowNestingLevel: number;
}

export function QuestionEditor(props: QuestionEditorProps)
{
    const { form, apiServiceState, questionId, isVisible, catalogId } = useSelector((state: RootState) => state.questionEditor);

    const dispatch = useDispatch();
    useEffect(() => { dispatch(fetchQuestion(questionId)) }, [questionId]);   

    const handleCancel = () => { dispatch(new CloseQuestionEditorWindow()) };  
    const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) =>  dispatch(new InputHasChanged(event, "Question"));

    return (!isVisible ? null :
        <Window level={props.windowNestingLevel} title="Question editor" onCancel={handleCancel} onOk={() => dispatch(saveChanges(questionId, form.data, catalogId!))} error={apiServiceState.apiError} isOkEnabled={!form.hasValidationErrors && !apiServiceState.isBusy}>
            <BusyIndicator isBusy={apiServiceState.isBusy}>
                {() =>
                    <MagicForm onInputChange={handleInputChange} conflictErrors={form.conflictErrors} validationErrors={form.validationErrors}>
                        <div className="form-group">
                            <MagicTextInput path="content" value={form.data.content} placeholder="Enter question content" />
                        </div>

                        <div className="form-group form-row">
                            <div className="col-sm-auto">
                            </div>
                            <div className="col">
                            </div>
                            <div className="col-sm-auto">
                            </div>
                        </div>

                        {form.data.answers.map((x, i) =>
                        {
                            return (
                                <div className="form-group form-row" key={i}>
                                    <div className="col-sm-auto d-flex">
                                        <div className="custom-control custom-checkbox justify-content-center align-self-center">
                                            <input name={`answers[${i}].isCorrect`} id={`answers[${i}].isCorrect`} checked={x.isCorrect} onChange={handleInputChange} type="checkbox" className="custom-control-input" />
                                            <label className="custom-control-label" htmlFor={`answers[${i}].isCorrect`}>&nbsp;</label>
                                        </div>
                                    </div>
                                    <div className="col">
                                        <MagicTextInput path={`answers[${i}].content`} value={x.content} />
                                    </div>
                                    <div className="col-sm-auto">
                                        <button className="btn btn-outline-danger btn-svg " onClick={preventDefault(() => dispatch(new DeleteAnswer(i)))} ><DeleteIco height="auto" /></button>
                                    </div>
                                </div>
                            )
                        }
                        )}
                        <div className="form-group" >
                            <button className="btn btn-outline-primary" onClick={preventDefault(() => dispatch(new AddAnswer()))}>add answer</button>
                        </div>
                    </MagicForm>
                }
            </BusyIndicator>
        </Window>
    );
}
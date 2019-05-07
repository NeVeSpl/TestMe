﻿import * as React from 'react';
import { QuestionsService, UpdateQuestion, ApiError, Answer, Question } from '../../../api';
import { Window, MagicForm, MagicDict, BusyIndicator, MagicTextInput } from '../../../components';
import { ReactComponent as DeleteIco }  from './outline-delete_forever-24px.svg';
import { ObjectUtils } from '../../../utils';

interface QuestionEditorProps
{
    catalogId: number;
    questionId?: number;
    onQuestionCreated?: (questionId: number) => void;
    onQuestionUpdated?: (questionId: number) => void;
    onCancel: () => void;
    windowNestingLevel: number;
}
class QuestionEditorState
{
    apiError: ApiError | undefined;
    isBusy: boolean;
    formData: UpdateQuestion;
    validationErrors: MagicDict;
    conflictErrors: MagicDict;
    hasValidationErrors: boolean;

    constructor()
    {
        this.isBusy = false;
        this.formData = new UpdateQuestion();
        this.validationErrors = {};
        this.conflictErrors = {};
        this.hasValidationErrors = false;       
    }
}

export default class QuestionEditor extends React.Component<QuestionEditorProps, QuestionEditorState>
{
    service: QuestionsService = new QuestionsService(x => this.setState({ apiError: x }), x => this.setState({ isBusy: x }), x => this.handleConcurrencyConflict());
    state = new QuestionEditorState();
    isFormItemTouched: Set<string> = new Set();
    originalFormData: Question | undefined;

    componentDidMount()
    {
        this.fetchQuestion(this.props.questionId);
    }
    componentDidUpdate(prevProps: QuestionEditorProps, prevState: QuestionEditorState, snapshot: any)
    {
        if (this.props.questionId !== prevProps.questionId)
        {
            this.fetchQuestion(this.props.questionId);
        }
    }

    fetchQuestion(questionId: number | undefined) : Promise<Question>
    {
        if (questionId !== undefined)
        {
            this.service.ReadQuestionWithAnswers(questionId)
                .then(x =>
                {
                    this.originalFormData = ObjectUtils.deepClone(x);
                    this.setState({ formData: { ...x } as unknown as UpdateQuestion });
                    return x;
                });
        }
        return Promise.reject();
    }

    handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => 
    {
        this.isFormItemTouched.add(event.target.name);
        const newFormData = MagicForm.ApplyEventTo(event, this.state.formData);
        this.validate(newFormData);
        this.setState({ formData: newFormData });      
    }
    async handleConcurrencyConflict()
    {        
        const userFormData = this.state.formData;
        const originalFormData = this.originalFormData!;
        const serverFormData = await this.service.ReadQuestionWithAnswers(this.props.questionId!);
        const result = MagicForm.ResolveConflicts(originalFormData, userFormData, serverFormData);
        result.resolvedFormData.concurrencyToken = serverFormData.concurrencyToken;
        this.setState({ formData: result.resolvedFormData, conflictErrors: result.conflictErrors });       
    }
   
    validate(data: UpdateQuestion, forceValidation: boolean = false)
    {        
        const validationErrors: MagicDict = {};
        if (this.isFormItemTouched.has('content') || forceValidation)  
        {
            if (data.content.length < 3)
            {
                validationErrors['content'] = "Content must be longer than 2 characters";
            }
            if (data.content.length > 2048)
            {
                validationErrors['content'] = "Content must be shorter than 2048 characters";
            }
        }
        for (let i = 0; i < data.answers.length; ++i)
        {
            const key = `answers[${i}].content`;
            if (this.isFormItemTouched.has(key) || forceValidation)  
            {
                if (data.answers[i].content.length < 3)
                {
                    validationErrors[key] = "Answer must be longer than 2 characters";
                }
                if (data.answers[i].content.length > 2048)
                {
                    validationErrors[key] = "Answer must be shorter than 2048 characters";
                }
            }
        }
        
        const hasValidationErrors = Object.keys(validationErrors).length > 0;
        this.setState({ validationErrors: validationErrors, hasValidationErrors: hasValidationErrors });   
        return hasValidationErrors;
    }    

    handleSaveChanges = () =>
    {
        const formData = this.state.formData;
        formData.catalogId = this.props.catalogId;
       
        if (this.validate(formData, true))
        {          
            return;
        }
        
        if (this.props.questionId === undefined)
        {
            this.service.CreateQuestionWithAnswers(formData)
                .then(x =>
                {
                    if (this.props.onQuestionCreated !== undefined)
                    {
                        this.props.onQuestionCreated(x);
                    }                   
                });
        }
        else
        {
            this.service.UpdateQuestionWithAnswers(this.props.questionId, formData)
                .then(x =>
                {                  
                    if (this.props.onQuestionUpdated !== undefined)
                    {
                        this.props.onQuestionUpdated(this.props.questionId!);
                    }                   
                });
        }
    }

    handleAddAnswer = (event: React.MouseEvent<HTMLElement>) =>
    { 
        event.preventDefault();
        const newData = { ...this.state.formData };
        newData.answers = [...this.state.formData.answers, new Answer()];
        this.setState({ formData: newData });
    }
    handleDeleteAnswer = (event: React.MouseEvent<HTMLElement>, answerIndex: number) =>
    {
        event.preventDefault();
        const newData = { ...this.state.formData };
        newData.answers = newData.answers.filter((x, i) => i != answerIndex);
        this.setState({ formData: newData });
    }


    render()
    {
        return (
            <Window level={this.props.windowNestingLevel} title="Question editor" onCancel={this.props.onCancel} onOk={this.handleSaveChanges} error={this.state.apiError} isOkEnabled={!this.state.hasValidationErrors && !this.state.isBusy}>                
                <BusyIndicator isBusy={this.state.isBusy}>
                    {this.renderForm}
                </BusyIndicator>
            </Window>
        );
    }
    renderForm = () =>
    {
        return (
            <MagicForm onInputChange={this.handleInputChange} conflictErrors={this.state.conflictErrors} validationErrors={this.state.validationErrors}>
                <div className="form-group">
                    <MagicTextInput path="content" value={this.state.formData.content}  placeholder="Enter question content" />
                </div>

                <div className="form-group form-row">
                    <div className="col-sm-auto">                      
                    </div>
                    <div className="col">
                    </div>                   
                    <div className="col-sm-auto">
                    </div>
                </div>

                {this.state.formData.answers.map((x, i) =>
                {
                    return (
                        <div className="form-group form-row" key={i}>
                            <div className="col-sm-auto d-flex">
                                <div className="custom-control custom-checkbox justify-content-center align-self-center">
                                    <input name={`answers[${i}].isCorrect`} id={`answers[${i}].isCorrect`} checked={x.isCorrect} onChange={this.handleInputChange} type="checkbox" className="custom-control-input" />
                                    <label className="custom-control-label" htmlFor={`answers[${i}].isCorrect`}>&nbsp;</label>
                                </div>
                            </div>
                            <div className="col">
                                <MagicTextInput path={`answers[${i}].content`} value={x.content} />
                            </div>
                            <div className="col-sm-auto">
                                <button className="btn btn-outline-danger btn-svg " onClick={(e) => this.handleDeleteAnswer(e, i)} ><DeleteIco height="auto" /></button>
                            </div>
                        </div>
                    )
                }
                )}
                <div className="form-group" >
                    <button className="btn btn-outline-primary" onClick={(e) => this.handleAddAnswer(e)}>add answer</button>
                </div>
            </MagicForm>
        );
    }
}
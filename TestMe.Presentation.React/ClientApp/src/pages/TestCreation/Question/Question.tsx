import * as React from 'react';
import { BusyIndicator, Window, Prompt } from '../../../components';
import { QuestionsService, Question as QuestionDTO, ApiError, QuestionHeader } from '../../../api';
import { QuestionEditor } from '.';
import style from './Question.module.css';

interface QuestionProps 
{
    catalogId: number;
    questionId: number;
    onCancel: () => void;
    onQuestionDeleted: (questionId: number) => void;
    onQuestionUpdated: (questionId: number) => void;
    windowNestingLevel: number;
}
class QuestionState
{
    question: QuestionDTO;
    isBusy: boolean;
    apiError: ApiError | undefined;
    openedChildWindow: ChildWindows;

    constructor()
    {
        this.question = new QuestionDTO();
        this.isBusy = true;
        this.openedChildWindow = ChildWindows.None;
    }
}
enum ChildWindows { None, QuestionDeletePrompt, QuestionEditor }

export default class Question extends React.Component<QuestionProps, QuestionState>
{
    readonly service: QuestionsService = new QuestionsService(x => this.setState({ apiError: x }), x => this.setState({ isBusy: x }));
    readonly state: QuestionState = new QuestionState();

    componentDidMount()
    {
        this.fetchQuestion(this.props.questionId);        
    } 
    componentDidUpdate(prevProps: QuestionProps, prevState: QuestionState, snapshot: any)
    {
        if (this.props.questionId !== prevProps.questionId)
        {
            this.fetchQuestion(this.props.questionId);           
        }
    }

    fetchQuestion(questionId: number)
    {
        this.service.ReadQuestionWithAnswers(questionId).then((x) => this.setState({ question: x}));      
    }

    setOpenedChildWindow = (event: React.MouseEvent<HTMLElement> | null, childWindow: ChildWindows) =>
    {
        if (event != null)
        {
            event.preventDefault();
        }
        this.setState({ openedChildWindow: childWindow });
    }


    handleDeleteQuestion = () =>
    {
        this.setOpenedChildWindow(null, ChildWindows.None);
        this.service.DeleteQuestionWithAnswers(this.props.questionId)
            .then(x =>
            {
                    this.props.onQuestionDeleted(this.props.questionId);               
            });
    }
    handleQuestionUpdated = (questionId: number) =>
    {
        this.setOpenedChildWindow(null, ChildWindows.None);
        this.props.onQuestionUpdated(questionId)
        this.fetchQuestion(questionId);
    }

    render()
    {
        return (
            <>
                <Window level={this.props.windowNestingLevel} title={"Question : " + this.state.question.content} onCancel={this.props.onCancel} onOk={this.props.onCancel} error={this.state.apiError} isEnabled={this.state.openedChildWindow == ChildWindows.None}>
                    <div className="text-right mb-3" >
                        <button type="button" className="btn btn-outline-danger mr-1" onClick={() => this.setOpenedChildWindow(null, ChildWindows.QuestionDeletePrompt)} >Delete question</button>
                        <button type="button" className="btn btn-outline-info" onClick={() => this.setOpenedChildWindow(null, ChildWindows.QuestionEditor)}>Edit question</button>
                    </div>
                    <BusyIndicator isBusy={this.state.isBusy}>
                        {this.renderQuestion}
                    </BusyIndicator>
                </Window>
                {this.renderChildWindow()}
            </>    
            );
    }
    renderQuestion = () =>
    {
        return (
            <>
                <p>{this.state.question.content}</p>

            <ul>{this.state.question.answers.map(x =>
            {
                const className = x.isCorrect ? style.correct : style.incorrect;
                return <li className={className} key={x.answerId}>{x.content}</li>;
               
            })
                }
                </ul>
            </>
                );
    }
    renderChildWindow = () =>
    {
        switch (this.state.openedChildWindow)
        {
            case ChildWindows.QuestionDeletePrompt:
                return (
                    <Prompt
                        level={this.props.windowNestingLevel + 1}
                        onOk={this.handleDeleteQuestion}
                        message="Are you sure?"
                        onCancel={() => this.setOpenedChildWindow(null, ChildWindows.None)}
                    />
                );
            case ChildWindows.QuestionEditor:
                return (
                    <QuestionEditor
                        windowNestingLevel={this.props.windowNestingLevel + 1}
                        catalogId={this.props.catalogId}
                        questionId={this.props.questionId}
                        onCancel={() => this.setOpenedChildWindow(null, ChildWindows.None)}
                        onQuestionUpdated={this.handleQuestionUpdated}
                    />
                );
        }
        return;
    }
}
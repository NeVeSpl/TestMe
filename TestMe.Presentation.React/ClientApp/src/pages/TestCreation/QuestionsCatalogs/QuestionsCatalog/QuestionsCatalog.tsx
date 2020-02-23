import * as React from 'react';
import { ArrayUtils, StringUtils, StateStorage } from '../../../../utils';
import { BusyIndicator, Window, Prompt } from '../../../../components';
import { Question, QuestionsCatalogEditor, QuestionEditor } from '../';
import { QuestionsService, ApiError, QuestionHeaderDTO } from '../../../../autoapi/services/QuestionsService';
import { QuestionsCatalogsService, CatalogDTO } from '../../../../autoapi/services/QuestionsCatalogsService';

export interface QuestionsCatalogProps 
{
    injectedStorage?: StateStorage<QuestionsCatalogState>;
    injectedQuestionService?: QuestionsService;
    injectedCatalogService?: QuestionsCatalogsService;

    catalogId: number;
    onCatalogDeleted: (catalogId: number) => void; 
    onCatalogUpdated: (catalogId: number) => void;
    onCancel: () => void;
    windowNestingLevel: number;
}
enum ChildWindows { None, QuestionsCatalogEditor, QuestionEditor, Question, QuestionsCatalogDeletePrompt }
export class QuestionsCatalogState
{   
    catalog: CatalogDTO;
    questions: QuestionHeaderDTO[];
    catalogsApiError: ApiError | undefined;
    questionsApiError: ApiError | undefined;
    catalogsIsBusy: boolean;
    questionsIsBusy: boolean;
    openedQuestionId: number;
    openedChildWindow: ChildWindows;

    constructor()
    {
        this.catalog = new CatalogDTO();
        this.questions = [];
        this.catalogsIsBusy = false;
        this.questionsIsBusy = false;
        this.openedQuestionId = 0;
        this.openedChildWindow = ChildWindows.None;
    }
}


export default class QuestionsCatalog extends React.Component<QuestionsCatalogProps, QuestionsCatalogState>
{
    readonly storage: StateStorage<QuestionsCatalogState> = this.props.injectedStorage ?? new StateStorage(QuestionsCatalogState, "QuestionsCatalogState");
    readonly questionService: QuestionsService = this.props.injectedQuestionService ?? new QuestionsService(x => this.setState({ questionsApiError: x }), x => this.setState({ questionsIsBusy: x }));
    readonly catalogService: QuestionsCatalogsService = this.props.injectedCatalogService ?? new QuestionsCatalogsService(x => this.setState({ catalogsApiError: x }), x => this.setState({ catalogsIsBusy: x }));
    readonly state = this.storage?.Load() ?? new QuestionsCatalogState();    

    componentDidMount()
    {
        this.fetchCatalog(this.props.catalogId);
        this.fetchQuestions(this.props.catalogId);       
    }
    componentDidUpdate(prevProps: QuestionsCatalogProps, prevState: QuestionsCatalogState, snapshot: any)
    {
        if (this.props.catalogId !== prevProps.catalogId)
        {
            this.fetchCatalog(this.props.catalogId);
            this.fetchQuestions(this.props.catalogId);
        }
        this.storage?.Save(this.state);
    }


    fetchCatalog(catalogId: number)
    {        
        this.catalogService.readQuestionsCatalog(catalogId).then(x => this.setState({ catalog: x}));
    }
    fetchQuestions(catalogId: number)
    {
        this.questionService.readQuestionHeaders(catalogId, { limit : 10, offset : 0 }).then(x => this.setState({ questions: x.result }));        
    }  
    async fetchQuestion(questionId: number)
    {
        return await this.questionService.readQuestionHeader(questionId);
    }

    setOpenedChildWindow = (event: React.MouseEvent<HTMLElement> | null, childWindow: ChildWindows) =>
    {
        if (event != null)
        {
            event.preventDefault();
        }
        this.setState({ openedChildWindow: childWindow });
    }   
    showQuestion = (event: React.MouseEvent<HTMLElement> | null, questionId: number) =>
    {
        this.setState({ openedQuestionId: questionId });
        this.setOpenedChildWindow(event, ChildWindows.Question);
    } 

    handleCatalogUpdated = (updatedCatalogId: number) =>
    {
        this.setOpenedChildWindow(null, ChildWindows.None);
        this.props.onCatalogUpdated(updatedCatalogId);
        this.fetchCatalog(updatedCatalogId);  
    }
    handleDeleteCatalog = () =>
    {
        this.setOpenedChildWindow(null, ChildWindows.None);
        this.catalogService.deleteCatalog(this.props.catalogId)
            .then(x =>
            {
                this.props.onCatalogDeleted(this.props.catalogId);                
            });       
    }

    handleQuestionCreated = async (createdQuestionId: number) =>
    {
        this.setOpenedChildWindow(null, ChildWindows.None);
        const createdQuestion = await this.fetchQuestion(createdQuestionId);
        this.setState({ questions: [...this.state.questions, createdQuestion] });        
    }
    handleQuestionUpdated = async (updatedQuestionId: number) =>
    {      
        const updatedQuestion = await this.fetchQuestion(updatedQuestionId);
        this.setState({ questions: ArrayUtils.ReplaceFirst(this.state.questions, x => x.questionId === updatedQuestionId, updatedQuestion) });       
    }
    handleQuestionDeleted = (questionId: number) => 
    {       
        this.setOpenedChildWindow(null, ChildWindows.None);
        this.setState({ questions: this.state.questions.filter(x => x.questionId !== questionId) });        
    }  
       
    render()
    {
        return (
            <>
                <Window level={this.props.windowNestingLevel} title={"Catalog : " + this.state.catalog.name} onCancel={this.props.onCancel} onOk={this.props.onCancel} error={this.state.catalogsApiError || this.state.questionsApiError} isEnabled={this.state.openedChildWindow === ChildWindows.None}>
                    <div className="text-right mb-3" >
                        <button type="button" className="btn btn-outline-danger mr-1" onClick={(e) => this.setOpenedChildWindow(e, ChildWindows.QuestionsCatalogDeletePrompt)}>Delete catalog</button>
                        <button type="button" className="btn btn-outline-info m-0" onClick={(e) => this.setOpenedChildWindow(e, ChildWindows.QuestionsCatalogEditor)}>Edit catalog</button>
                    </div>
                    <BusyIndicator isBusy={this.state.questionsIsBusy || this.state.catalogsIsBusy}>
                        {this.renderQuestionsHeaders}
                    </BusyIndicator>
                </Window>
                {this.renderChildWindow()}
            </>
        );
    }
    renderQuestionsHeaders = () =>
    {
        return (
            <div className="list-group">
                {this.state.questions.sort((a, b) => a.content.localeCompare(b.content))
                    .map(x => <a
                        className="list-group-item list-group-item-action"
                        href="#"
                        key={x.questionId}
                        onClick={(e) => this.showQuestion(e, x.questionId)}>{StringUtils.Truncate40(x.content)}</a>)}
                <button type="button" className="list-group-item list-group-item-action list-group-item-primary text-center" onClick={(e) => this.setOpenedChildWindow(e, ChildWindows.QuestionEditor)}>Add new question</button>
            </div>
        );
    }
    renderChildWindow = () =>
    {
        switch (this.state.openedChildWindow)
        {
            case ChildWindows.QuestionsCatalogDeletePrompt:
                return (
                    <Prompt
                        level={this.props.windowNestingLevel + 1}
                        onOk={this.handleDeleteCatalog}
                        message="Are you sure?"
                        onCancel={() => this.setOpenedChildWindow(null, ChildWindows.None)}
                    />
                );
            case ChildWindows.Question:
                return (
                    <Question
                        windowNestingLevel={this.props.windowNestingLevel + 1}
                        catalogId={this.props.catalogId}
                        questionId={this.state.openedQuestionId}
                        onCancel={() => this.setOpenedChildWindow(null, ChildWindows.None)}
                        onQuestionUpdated={this.handleQuestionUpdated}
                        onQuestionDeleted={this.handleQuestionDeleted}
                    />
                );
            case ChildWindows.QuestionsCatalogEditor:
                return (
                    <QuestionsCatalogEditor
                        windowNestingLevel={this.props.windowNestingLevel + 1}
                        catalogId={this.props.catalogId}
                        onCancel={() => this.setOpenedChildWindow(null, ChildWindows.None)}
                        onCatalogUpdated={this.handleCatalogUpdated}
                    />
                );
            case ChildWindows.QuestionEditor:
                return (
                    <QuestionEditor
                        windowNestingLevel={this.props.windowNestingLevel + 1}
                        catalogId={this.props.catalogId}
                        onCancel={() => this.setOpenedChildWindow(null, ChildWindows.None)}
                        onQuestionCreated={this.handleQuestionCreated}
                    />
                );
        }
        return;
    }   
}
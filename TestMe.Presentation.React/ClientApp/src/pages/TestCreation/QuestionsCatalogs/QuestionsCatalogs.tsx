import * as React from 'react';
import { ArrayUtils, StateStorage } from '../../../utils';
import { BusyIndicator, Window } from '../../../components';
import { QuestionsCatalog, QuestionsCatalogEditor } from '.';
import { UserService } from '../../../services';
import { ApiError, QuestionsCatalogsService, CatalogHeaderDTO, OffsetPagination } from '../../../autoapi/services/QuestionsCatalogsService';

interface QuestionsCatalogsProps
{
    injectedStorage?: StateStorage<QuestionsCatalogsState>;
    injectedService?: QuestionsCatalogsService;
}
enum ChildWindows { None, QuestionsCatalogEditor, QuestionsCatalog }
export class QuestionsCatalogsState
{
    questionsCatalogs: CatalogHeaderDTO[];
    isBusy: boolean;
    apiError: ApiError | undefined;  
    openedQuestionsCatalogId: number;
    openedChildWindow: ChildWindows;

    constructor()
    {      
        this.questionsCatalogs = [];
        this.isBusy = false;
        this.openedQuestionsCatalogId = 0;
        this.openedChildWindow = ChildWindows.None;
    }
}


export default class QuestionsCatalogs extends React.Component<QuestionsCatalogsProps, QuestionsCatalogsState>
{
    readonly storage: StateStorage<QuestionsCatalogsState> = this.props.injectedStorage ?? new StateStorage(QuestionsCatalogsState, "QuestionsCatalogsState");
    readonly service: QuestionsCatalogsService = this.props.injectedService ?? new QuestionsCatalogsService(x => this.setState({ apiError: x }), x => this.setState({ isBusy: x }));
    readonly state = this.storage?.Load() ?? new QuestionsCatalogsState();    
   

    componentDidMount()
    {
        this.fetchCatalogs();
    }
    componentDidUpdate()
    {
        this.storage?.Save(this.state);
    }

    fetchCatalogs()
    {
        this.service.readQuestionsCatalogHeaders(UserService.getUserID(), { limit : 10, offset : 0 } ).then(x => this.setState({ questionsCatalogs: x.result }));
    }
    async fetchCatalog(catalogId : number)
    {     
        return await this.service.readQuestionsCatalogHeader(catalogId);
    }

    setOpenedChildWindow = (event : React.MouseEvent<HTMLElement> | null, childWindow : ChildWindows) =>
    {
        if (event != null)
        {           
            event.preventDefault();
        }
        this.setState({ openedChildWindow: childWindow });
    }
    showQuestionsCatalog = (event : React.MouseEvent<HTMLElement> | null, catalogId : number) =>
    {        
        this.setState({ openedQuestionsCatalogId: catalogId });
        this.setOpenedChildWindow(event, ChildWindows.QuestionsCatalog);
    }   

    
    handleCatalogCreated = async (createdCatalogId: number) =>
    {
        this.setOpenedChildWindow(null, ChildWindows.None);
        const createdCatalog = await this.fetchCatalog(createdCatalogId);
        this.setState({ questionsCatalogs: [...this.state.questionsCatalogs, createdCatalog] });       
    }
    handleCatalogUpdated = async (updatedCatalogId: number) =>
    {       
        const updatedCatalog = await this.fetchCatalog(updatedCatalogId);
        this.setState({ questionsCatalogs: ArrayUtils.ReplaceFirst(this.state.questionsCatalogs, x => x.catalogId === updatedCatalogId, updatedCatalog)});
    }
    handleCatalogDeleted = (catalogId: number) =>
    {  
        this.setOpenedChildWindow(null, ChildWindows.None);
        this.setState({ questionsCatalogs: this.state.questionsCatalogs.filter(x => x.catalogId !== catalogId) });        
    } 

    render()
    {     
        return (
            <>
                <Window title="Catalogs of questions" error={this.state.apiError} isEnabled={this.state.openedChildWindow === ChildWindows.None} >
                    <BusyIndicator isBusy={this.state.isBusy}>
                        {this.renderCatalogs}
                    </BusyIndicator>
                </Window>
                {this.renderChildWindow()}               
            </>
        );
    }
    renderCatalogs = () =>
    {
        return (
            <div className="list-group">
                {this.state.questionsCatalogs.sort((a, b) => a.name.localeCompare(b.name)).map(x =>
                    <a className="list-group-item list-group-item-action" href="#" key={x.catalogId} onClick={(e) => this.showQuestionsCatalog(e, x.catalogId)}>{x.name}</a>                   
                )}
                <button className="list-group-item list-group-item-action list-group-item-primary text-center" onClick={(e) => this.setOpenedChildWindow(e, ChildWindows.QuestionsCatalogEditor)} >Add new catalog</button>
            </div>    
        )
    }
    renderChildWindow = () =>
    {
        switch (this.state.openedChildWindow)
        {
            case ChildWindows.QuestionsCatalog:
                return (
                    <QuestionsCatalog
                        windowNestingLevel={1}
                        catalogId={this.state.openedQuestionsCatalogId}
                        onCancel={() => this.setOpenedChildWindow(null, ChildWindows.None)}
                        onCatalogDeleted={this.handleCatalogDeleted}
                        onCatalogUpdated={this.handleCatalogUpdated}
                    />
                    );                
            case ChildWindows.QuestionsCatalogEditor:
                return (
                    <QuestionsCatalogEditor
                        windowNestingLevel={1}
                        onCancel={() => this.setOpenedChildWindow(null, ChildWindows.None)}
                        onCatalogCreated={this.handleCatalogCreated} />
                    );
        }
        return;
    }
}
import * as React from 'react';
import { ArrayUtils, StateStorage } from '../../../utils';
import { BusyIndicator, Window, Pagination } from '../../../components';
import { QuestionsCatalog, QuestionsCatalogEditor } from '../';
import { UserService } from '../../../services';
import { ApiError, QuestionsCatalogsService, CatalogOnListDTO } from '../../../autoapi/services/QuestionsCatalogsService';

interface QuestionsCatalogsProps
{
    injectedStorage?: StateStorage<QuestionsCatalogsState>;
    injectedService?: QuestionsCatalogsService;
}
enum ChildWindows { None, QuestionsCatalogEditor, QuestionsCatalog }
export class QuestionsCatalogsState
{
    questionsCatalogs: CatalogOnListDTO[];
    isBusy: boolean;
    apiError: ApiError | undefined;  
    openedQuestionsCatalogId: number;
    openedChildWindow: ChildWindows;
    canShiftRight: boolean;
    currentPage: number;

    constructor()
    {      
        this.questionsCatalogs = [];
        this.isBusy = false;
        this.openedQuestionsCatalogId = 0;
        this.openedChildWindow = ChildWindows.None;
        this.canShiftRight = false;
        this.currentPage = 1;
    }
}

const ItemsPerPage = 5;

export default class QuestionsCatalogs extends React.Component<QuestionsCatalogsProps, QuestionsCatalogsState>
{
    readonly storage: StateStorage<QuestionsCatalogsState> = this.props.injectedStorage ?? new StateStorage<QuestionsCatalogsState>("QuestionsCatalogsState");
    readonly service: QuestionsCatalogsService = this.props.injectedService ?? new QuestionsCatalogsService(x => this.setState({ apiError: x }), x => this.setState({ isBusy: x }));
    readonly state = this.storage?.Load() ?? new QuestionsCatalogsState();    
  

    componentDidMount()
    {
        this.fetchCatalogs(this.state.currentPage);
    }
    componentDidUpdate()
    {
        this.storage?.Save(this.state);
    }

    fetchCatalogs(pageNumber : number)
    {
        this.service.readQuestionsCatalogs(UserService.getUserID(), { limit: ItemsPerPage, offset: (pageNumber - 1 ) * ItemsPerPage }).then(x => this.setState({ questionsCatalogs: x.result, canShiftRight: x.isThereMore }));
    }
    async fetchCatalog(catalogId : number)
    {     
        return await this.service.readQuestionsCatalog(catalogId);
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
        this.fetchCatalogs(this.state.currentPage);       
    }
    handleCatalogUpdated = async (updatedCatalogId: number) =>
    {       
        const updatedCatalog = await this.fetchCatalog(updatedCatalogId);
        this.setState({ questionsCatalogs: ArrayUtils.ReplaceFirst(this.state.questionsCatalogs, x => x.catalogId === updatedCatalogId, updatedCatalog)});
    }
    handleCatalogDeleted = (catalogId: number) =>
    {  
        this.setOpenedChildWindow(null, ChildWindows.None);
        this.fetchCatalogs(this.state.currentPage);     
    } 
    handleShift = (newPage: number) =>
    {
        this.setState({ currentPage: newPage });
        this.fetchCatalogs(newPage);
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
            <>
                <div className="list-group">
                    {this.state.questionsCatalogs.map(x =>
                        <a className="list-group-item list-group-item-action" href="#" key={x.catalogId} onClick={(e) => this.showQuestionsCatalog(e, x.catalogId)}>{x.name}</a>                   
                    )}
                   
                </div>
                <div className="d-flex justify-content-center mt-3">
                    <Pagination canShiftRight={this.state.canShiftRight} onShiftRight={this.handleShift} onShiftLeft={this.handleShift} />
                </div>
                <div className="mt-0">
                    <button className="btn btn-outline-primary" onClick={(e) => this.setOpenedChildWindow(e, ChildWindows.QuestionsCatalogEditor)} >Add catalog</button>
                </div>
            </>
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
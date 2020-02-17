import * as React from 'react';
import { Window, TextInput, ObjectForm, BusyIndicator, ObjectFormItem } from '../../../components';
import { QuestionsCatalogsService, ApiError, CreateCatalogDTO, UpdateCatalogDTO } from '../../../autoapi/services/QuestionsCatalogsService';

class QuestionsCatalogEditorForm
{    
    name: ObjectFormItem;

    constructor()
    {
        this.name = new ObjectFormItem()
    }
}
interface QuestionsCatalogEditorProps
{
    catalogId?: number;
    onCatalogCreated?: (catalogId: number) => void;
    onCatalogUpdated?: (catalogId: number) => void;
    onCancel: () => void;
    windowNestingLevel: number;
}
class QuestionsCatalogEditorState
{  
    apiError: ApiError | undefined;
    isBusy: boolean;
    hasValidationErrors: boolean;
    form: QuestionsCatalogEditorForm;

    constructor()
    {
        this.isBusy = false;     
        this.hasValidationErrors = false;
        this.form = new QuestionsCatalogEditorForm();
    }
}


export default class QuestionsCatalogEditor extends React.Component<QuestionsCatalogEditorProps, QuestionsCatalogEditorState>
{
    service: QuestionsCatalogsService = new QuestionsCatalogsService(x => this.setState({ apiError: x }), x => this.setState({ isBusy : x }));
    state = new QuestionsCatalogEditorState();
    

    componentDidMount()
    {
       this.fetchCatalog(this.props.catalogId);       
    }
    componentDidUpdate(prevProps: QuestionsCatalogEditorProps, prevState: QuestionsCatalogEditorState, snapshot: any)
    {
        if (this.props.catalogId !== prevProps.catalogId)
        {
            this.fetchCatalog(this.props.catalogId);
        }
    }

    fetchCatalog(catalogId: number | undefined)
    {
        if (catalogId !== undefined)
        {
            this.service.readQuestionsCatalog(catalogId)
                .then(x =>
                {
                    this.setState({ form: ObjectForm.mapToForm(x, new QuestionsCatalogEditorForm()) });
                });
        }
    }

    handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => 
    {
        this.updateAndValidate();
    }  
    updateAndValidate = (forceValidation: boolean = false): boolean =>
    {
        const formCopy = ObjectForm.applyChangesAndResetErrors(this.state.form);
        this.validate(formCopy, forceValidation);
        const hasValidationErrors = ObjectForm.hasValidationErrors(formCopy);
        this.setState({ form: formCopy, hasValidationErrors: hasValidationErrors });
        return hasValidationErrors;
    }
    validate(data: QuestionsCatalogEditorForm, forceValidation: boolean = false)
    {
        if ((data.name.isTouched) || (forceValidation))
        {
            if (data.name.value.length < 3)
            {
                data.name.error = "Name must be longer than 2 characters";
            }
            if (data.name.value.length > 2048)
            {
                data.name.error = "Name must be shorter than 2048 characters";
            }
        }        
    }
    


    handleSaveChanges = () =>
    {        
        if (this.updateAndValidate(true))
        {           
            return;
        }

        if (this.props.catalogId === undefined)
        {
            this.service.createCatalog(ObjectForm.mapToDTO(this.state.form, new CreateCatalogDTO()))
                .then(x => 
                {                  
                    if (this.props.onCatalogCreated !== undefined)
                    {
                        this.props.onCatalogCreated(x);
                    }
                });
        }
        else
        {
            this.service.updateCatalog(this.props.catalogId, ObjectForm.mapToDTO(this.state.form, new UpdateCatalogDTO()))
                .then(x =>
                {                    
                    if (this.props.onCatalogUpdated !== undefined)
                    {
                        this.props.onCatalogUpdated(this.props.catalogId!);
                    }
                });
        }
    }

    render()
    {
        return (
            <Window level={this.props.windowNestingLevel} title="Catalog editor" onCancel={this.props.onCancel} onOk={this.handleSaveChanges} error={this.state.apiError} isOkEnabled={!this.state.hasValidationErrors && !this.state.isBusy} >
                <BusyIndicator isBusy={this.state.isBusy}>
                    {this.renderForm}
                </BusyIndicator>
            </Window>
        );
    }
    renderForm = () =>
    {
        return (
            <form>
                <div className="form-group">
                    <TextInput label="Name" name="name" onInputChange={this.handleInputChange} data={this.state.form.name} />
                </div>
            </form>
        );
    }
}
import * as React from 'react';
import { Window, BusyIndicator, FormikTextInput } from '../../../../components';
import { QuestionsCatalogsService, ApiError, CreateCatalogDTO, UpdateCatalogDTO } from '../../../../autoapi/services/QuestionsCatalogsService';
import { Formik, FormikProps, FormikHelpers } from 'formik';
import { StateStorage } from '../../../../utils';

interface QuestionsCatalogEditorProps
{
    injectedStorage?: StateStorage<QuestionsCatalogEditorState>;
    injectedService?: QuestionsCatalogsService;

    catalogId?: number;
    onCatalogCreated?: (catalogId: number) => void;
    onCatalogUpdated?: (catalogId: number) => void;
    onCancel: () => void;
    windowNestingLevel: number;
}
export class QuestionsCatalogEditorState {
    apiError: ApiError | undefined;
    isBusy: boolean;
    hasValidationErrors: boolean;
    formData: CreateCatalogDTO;

    constructor() {
        this.isBusy = false;
        this.hasValidationErrors = false;
        this.formData = new CreateCatalogDTO();
    }
}

export default class QuestionsCatalogEditor extends React.Component<QuestionsCatalogEditorProps, QuestionsCatalogEditorState>
{
    readonly storage: StateStorage<QuestionsCatalogEditorState> = this.props.injectedStorage ?? new StateStorage<QuestionsCatalogEditorState>("QuestionsCatalogEditorState");
    readonly service: QuestionsCatalogsService = this.props.injectedService ?? new QuestionsCatalogsService(x => this.setState({ apiError: x }), x => this.setState({ isBusy: x }));
    readonly state = this.storage?.Load() ?? new QuestionsCatalogEditorState();
    invokeSubmit?: () => {} | null;

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
        this.storage?.Save(this.state);
    }

    fetchCatalog(catalogId: number | undefined) {
        if (catalogId !== undefined) {
            this.service.readQuestionsCatalog(catalogId)
                .then(x => {
                    this.setState({ formData: x });
                });
        }
    }

    validate = (data: CreateCatalogDTO) =>
    {
        const errors: object & { [key: string]: any } = {};

        if (data.name.length < 3) {
            errors.name = "Name must be longer than 2 characters";
        }
        if (data.name.length > 2048) {
            errors.name = "Name must be shorter than 2048 characters";
        }

        this.setState({ hasValidationErrors: Object.keys(errors).length !== 0 });

        return errors;
    }


    handleOk = () =>
    {
        this.invokeSubmit!();
    }

    handleSubmit = (values: CreateCatalogDTO, formikHelpers: FormikHelpers<CreateCatalogDTO>) =>
    {
        if (this.props.catalogId === undefined)
        {
            this.service.createCatalog(values)
                .then(x => {
                    this.storage.Erase();
                    this.props.onCatalogCreated?.(x)
                });
        }
        else
        {
            this.service.updateCatalog(this.props.catalogId, values)              
                .then(x => {      
                    this.storage.Erase()
                    this.props.onCatalogUpdated?.(this.props.catalogId!);                     
                });
        }
    }
    handleCancel = () => {
        this.storage.Erase();
        this.props.onCancel();
    }


    render() {
        return (
            <Window level={this.props.windowNestingLevel} title="Catalog editor" onCancel={this.handleCancel} onOk={this.handleOk} error={this.state.apiError} isOkEnabled={!this.state.hasValidationErrors && !this.state.isBusy} >
                <BusyIndicator isBusy={this.state.isBusy}>
                    {this.renderForm}
                </BusyIndicator>
            </Window>
        );
    }
    renderForm = () => {
        return (
            <Formik enableReinitialize={true} onSubmit={this.handleSubmit} validate={this.validate} initialValues={this.state.formData}>
                {
                    (formik: FormikProps<CreateCatalogDTO>) =>
                    {
                        this.invokeSubmit = formik.submitForm;                        
                        return (
                            <form>
                                <div className="form-group">
                                    <FormikTextInput name="name" formik={formik} label="Name" />
                                </div>
                            </form>
                        )
                    }
                }
            </Formik>
        );
    }
}
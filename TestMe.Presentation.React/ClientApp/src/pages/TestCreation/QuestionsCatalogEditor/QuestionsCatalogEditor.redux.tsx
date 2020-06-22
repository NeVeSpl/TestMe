import * as React from 'react';
import { Window, BusyIndicator, FormikTextInput } from '../../../components';
import { useSelector, useDispatch } from 'react-redux';
import { Formik, FormikProps } from 'formik';
import { RootState } from '../../../redux.base';
import { CreateCatalogDTO } from '../../../autoapi/services/QuestionsCatalogsService';
import { fetchCatalog, submitCatalog, CloseQuestionsCatalogEditorWindow } from './QuestionsCatalogEditor.reducer';


interface QuestionsCatalogEditorProps
{  
    windowNestingLevel: number;
}

function validate(setValidationErrors: React.Dispatch<React.SetStateAction<boolean>>) 
{
    return (data: CreateCatalogDTO) =>
    {
        const errors: object & { [key: string]: any } = {};

        if (data.name.length < 3)
        {
            errors.name = "Name must be longer than 2 characters";
        }
        if (data.name.length > 2048)
        {
            errors.name = "Name must be shorter than 2048 characters";
        }

        setValidationErrors(Object.keys(errors).length !== 0 );

        return errors;
    }
}

export function QuestionsCatalogEditor(props: QuestionsCatalogEditorProps)
{
    let invokeSubmit : (() => { } | null) | null = null;
    const state = useSelector((state: RootState) => state.questionsCatalogEditor);   
    const dispatch = useDispatch();
    React.useEffect(() => { dispatch(fetchCatalog(state.catalogId)) }, [state.catalogId]);
    const handleSubmit = (values: CreateCatalogDTO) => { dispatch(submitCatalog(state.catalogId, values)); };
    const handleCancel = () => { dispatch(new CloseQuestionsCatalogEditorWindow()) };
    const [hasValidationErrors, setValidationErrors] = React.useState(false);

    return (
        !state.isVisible ? null :
        <Window level={props.windowNestingLevel} title="Catalog editor" onCancel={handleCancel} onOk={ ()=>invokeSubmit!()} error={state.apiServiceState.apiError} isOkEnabled={!hasValidationErrors && !state.apiServiceState.isBusy} >
            <BusyIndicator isBusy={state.apiServiceState.isBusy}>
                {() =>                
                    <Formik enableReinitialize={true} onSubmit={handleSubmit} validate={validate(setValidationErrors)} initialValues={state.form}>
                        {
                            (formik: FormikProps<CreateCatalogDTO>) =>
                            {
                                invokeSubmit = formik.submitForm;
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
                
                }
            </BusyIndicator>
        </Window>
    );
}
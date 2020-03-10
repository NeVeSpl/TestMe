import * as React from 'react';
import { Window, BusyIndicator, FormikTextInput } from '../../../../components';
import { useSelector, useDispatch } from 'react-redux';
import { Formik, FormikProps, FormikHelpers } from 'formik';
import { RootState } from '../../../../redux.base';
import { QuestionsCatalogsService, ApiError, CreateCatalogDTO, UpdateCatalogDTO } from '../../../../autoapi/services/QuestionsCatalogsService';
import { fetchCatalog } from '..';
import { submitCatalog } from './QuestionsCatalogEditor.reducer';
import { CloseWindow, ChildWindows } from '../QuestionsCatalogs.reducer';

interface QuestionsCatalogEditorProps
{   
    catalogId?: number;     
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
    React.useEffect(() => { dispatch(fetchCatalog(props.catalogId)) }, [props.catalogId]);
    const handleSubmit = (values: CreateCatalogDTO) => { dispatch(submitCatalog(props.catalogId, values)); };
    const handleCancel = () => { dispatch(new CloseWindow(ChildWindows.QuestionsCatalogEditor)) };
    const [hasValidationErrors, setValidationErrors] = React.useState(false);

    return (
        <Window level={props.windowNestingLevel} title="Catalog editor" onCancel={handleCancel} onOk={ ()=>invokeSubmit!()} error={state.apiError} isOkEnabled={!hasValidationErrors && !state.isBusy} >
            <BusyIndicator isBusy={state.isBusy}>
                {() =>                
                    <Formik enableReinitialize={true} onSubmit={handleSubmit} validate={validate(setValidationErrors)} initialValues={state.formData}>
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


import * as React from 'react';
import { Formik, FormikProps, FormikHelpers, FormikErrors } from 'formik';
import { FormikTextInput, ErrorBoundary } from '../../../components';
import { TokensService, LoginCredentialsDTO, ApiError } from '../../../autoapi/services/TokensService';

interface SigninProp
{
    onSubmit: (values: LoginCredentialsDTO, formikHelpers: FormikHelpers<LoginCredentialsDTO>) => void;
    onValidate: (values: LoginCredentialsDTO) => void | object;
    initialValues: LoginCredentialsDTO;
}

export function Signin( props: SigninProp )
{
    return (
        <Formik enableReinitialize={true} onSubmit={props.onSubmit} validate={props.onValidate} initialValues={props.initialValues}>
            {
                (formik: FormikProps<LoginCredentialsDTO>) =>
                {
                    return (
                        <form onSubmit={formik.handleSubmit}>
                            <div className="form-group">
                                <FormikTextInput type="email" name="email" formik={formik} label="Email address" />
                            </div>
                            <div className="form-group">
                                <FormikTextInput type="password" name="password" formik={formik} label="Password" />
                            </div>
                            <button type="submit" className="btn btn-primary" disabled={formik.isSubmitting}>Submit</button>
                        </form>
                    )
                }
            }
        </Formik>
    );
}
import * as React from 'react';
import { Formik, FormikProps, FormikHelpers, FormikErrors } from 'formik';
import { FormikTextInput } from '../../../components';
import { CSSTransition, SwitchTransition } from 'react-transition-group';
import './../LandingPage.css';
import { CreateUserDTO, ApiError } from '../../../autoapi/services/UsersService';

interface SignupProp
{
    onSubmit: (values: CreateUserDTO, formikHelpers: FormikHelpers<CreateUserDTO>) => void;
    onValidate: (values: CreateUserDTO) => void | object;
    initialValues: CreateUserDTO;
    userWasCreated: boolean;
    apiError?: ApiError | undefined
}

export function Signup(props: SignupProp)
{
    return (        
        <SwitchTransition mode="out-in">
            <CSSTransition key={props.userWasCreated ? "k1" : "k2"} timeout={300} unmountOnExit mountOnEnter classNames="sign-up-transition">
                {props.userWasCreated ? renderUserWasCreatedSuccessfullyAlert() : renderSignUpForm(props)}
            </CSSTransition>
        </SwitchTransition>                    
    );
}

function renderSignUpForm(props: SignupProp)
{
    return (
        <Formik enableReinitialize={true} onSubmit={props.onSubmit} validate={props.onValidate} initialValues={props.initialValues}  >
            {
                (formik: FormikProps<CreateUserDTO>) =>
                {
                    return (
                        <form onSubmit={formik.handleSubmit} autoComplete="off">
                            <div className="form-group">
                                <FormikTextInput type="text" name="name" formik={formik} apiError={props.apiError} label="Name" />
                            </div>
                            <div className="form-group">
                                <FormikTextInput type="email" name="emailAddress" formik={formik} apiError={props.apiError} label="Email address" />
                            </div>
                            <div className="form-group">
                                <FormikTextInput type="password" name="password" formik={formik} apiError={props.apiError} label="Password" />
                            </div>

                            <button type="submit" className="btn btn-primary" disabled={formik.isSubmitting}>Submit</button>
                        </form>
                    )
                }
            }
        </Formik>
    );
}

function renderUserWasCreatedSuccessfullyAlert()
{
    return (
        <>
            <div className="alert alert-success" role="alert">
                <h4 className="alert-heading">Well done!</h4>
                <p>Your account has been created successfully, now you can log in.</p>
                <hr />
                <p className="mb-0">&lt;--------------------------------------------------</p>
            </div>
        </>
    );
}
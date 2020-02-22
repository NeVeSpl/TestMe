import * as React from 'react';
import { RouteComponentProps } from 'react-router-dom';
import { UserService } from '../../services';
import { TokensService, LoginCredentialsDTO, ApiError } from '../../autoapi/services/TokensService';
import styles from './LandingPage.module.css';
import './LandingPage.css';
import { CreateUserDTO, UsersService } from '../../autoapi/services/UsersService';
import { Formik, FormikProps, FormikHelpers } from 'formik';
import { FormikTextInput, ErrorBoundary } from '../../components';
import { CSSTransition, SwitchTransition } from 'react-transition-group';


export class LandingPageState
{
    createUserApiError: ApiError | undefined;
    createUserIsBusy: boolean;
    createUserForm: CreateUserDTO;
    userCreated: boolean;
    loginCredentialsApiError: ApiError | undefined;
    loginCredentialsIsBusy: boolean;
    loginCredentialsForm: LoginCredentialsDTO;

    constructor()
    {
        this.createUserIsBusy = false;
        this.createUserForm = new CreateUserDTO();
        this.userCreated = false;
        this.loginCredentialsIsBusy = false;
        this.loginCredentialsForm = new LoginCredentialsDTO();
        this.loginCredentialsForm.email = "";
        this.loginCredentialsForm.password = "";
    }
}



export class LandingPage extends React.Component<RouteComponentProps, LandingPageState>
{
    tokensService = new TokensService(x => this.setState({ loginCredentialsApiError: x }), x => this.setState({ loginCredentialsIsBusy: x }));
    usersService = new UsersService(x => this.handleApiError(x), x => this.setState({ createUserIsBusy: x }));
    userService = new UserService();
    state = new LandingPageState();


    handleLogin = (values: LoginCredentialsDTO, formikHelpers: FormikHelpers<LoginCredentialsDTO>) =>
    {       
        this.tokensService.createToken(values)
            .then(x => 
            {
                UserService.setToken(x.token);
                this.props.history.push("/testcreation");
            })
            .catch(e =>
            {
                const apiError = e as ApiError;
                formikHelpers.setErrors(apiError.errors);
            })
            .finally(() => formikHelpers.setSubmitting(false));
    }
    validateLogin = (data: LoginCredentialsDTO) =>
    {

    }

    handleSignup = (values: CreateUserDTO, formikHelpers: FormikHelpers<CreateUserDTO>) =>
    {
        this.usersService.createUser(values)
            .then(x =>
            {
                this.setState({ userCreated: true });
            })
            .catch(e =>
            {
                const apiError = e as ApiError;
                formikHelpers.setErrors(apiError.errors);
            })
            .finally(() => formikHelpers.setSubmitting(false));
    }
    validateSignup = (data: CreateUserDTO) =>
    {
        this.setState({ loginCredentialsForm: { email: data.emailAddress, password: data.password } });
        //return this.state.createUserApiError?.errors;
    }
    handleApiError = (error: ApiError | undefined) =>
    {
        this.setState({ createUserApiError: error })
    }


    render()
    {
        return (
            <>
                <div className="card-deck">

                    <div className="card ">
                        <div className={`card-header ${styles.card}`}>
                            Log in
                        </div>
                        <div className="card-body">
                            <ErrorBoundary error={this.state.loginCredentialsApiError}>
                                <Formik enableReinitialize={true} onSubmit={this.handleLogin} validate={this.validateLogin} initialValues={this.state.loginCredentialsForm}>
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
                            </ErrorBoundary>
                        </div>
                    </div>



                    <div className="card">
                        <div className={`card-header ${styles.card}`}>
                            Sign up
                        </div>
                        <div className="card-body">
                            <SwitchTransition mode="out-in">
                                <CSSTransition key={this.state.userCreated ? "bar" : "foo"} timeout={300} unmountOnExit mountOnEnter classNames="my-node">
                                    <ErrorBoundary error={this.state.createUserApiError}>
                                        {this.state.userCreated ? this.renderUserCreated() : this.renderSignUpForm()}
                                    </ErrorBoundary>
                                </CSSTransition>
                            </SwitchTransition>


                            
                        </div>
                    </div>


                </div>


            </>
        );
    }

    renderSignUpForm()
    {
        return (
            <Formik enableReinitialize={true} onSubmit={this.handleSignup} validate={this.validateSignup} initialValues={this.state.createUserForm}>
                {
                    (formik: FormikProps<CreateUserDTO>) =>
                    {
                        return (
                            <form onSubmit={formik.handleSubmit} autoComplete="off">
                                <div className="form-group">
                                    <FormikTextInput type="text" name="name" formik={formik} label="Name" />
                                </div>
                                <div className="form-group">
                                    <FormikTextInput type="email" name="emailAddress" formik={formik} label="Email address" />

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

    renderUserCreated()
    {
        return (
            <>
                <div className="alert alert-success" role="alert">
                    <h4 className="alert-heading">Well done!</h4>
                    <p>Your account has been created successfully, now you can log in.</p>
                    <hr/>
                     <p className="mb-0">&lt;--------------------------------------------------</p>
                </div>
             
            </>
        );
    }
}

import * as React from 'react';
import { RouteComponentProps } from 'react-router-dom';
import { UserService } from '../../services';
import { TokensService, LoginCredentialsDTO, ApiError } from '../../autoapi/services/TokensService';
import styles from './LandingPage.module.css';
import { CreateUserDTO, UsersService } from '../../autoapi/services/UsersService';
import { FormikHelpers } from 'formik';
import { ErrorBoundary } from '../../components';
import { Signin } from './Signin/Signin';
import { Signup } from './Signup/Signup';


export class LandingPageState
{
    createUserApiError: ApiError | undefined;
    createUserIsBusy: boolean;
    createUserForm: CreateUserDTO;
    userWasCreated: boolean;
    loginCredentialsApiError: ApiError | undefined;
    loginCredentialsIsBusy: boolean;
    loginCredentialsForm: LoginCredentialsDTO;

    constructor()
    {
        this.createUserIsBusy = false;
        this.createUserForm = new CreateUserDTO();
        this.userWasCreated = false;
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
        this.setState({ createUserForm: values});
        this.usersService.createUser(values)
            .then(x =>
            {
                this.setState({ userWasCreated: true });
            })
            .catch(e =>
            {
                
            })
            .finally(() => formikHelpers.setSubmitting(false));
    }
    validateSignup = (data: CreateUserDTO) =>
    {        
        //if (data.emailAddress)  todo: add debouncing
        //{
        //    return this.usersService.isEmailAddressTaken(data.emailAddress)
        //        .then(x => x ? { "emailAddress": "User with given email already exists." } : {});
        //}          
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
                                <Signin onSubmit={this.handleLogin} onValidate={this.validateLogin} initialValues={this.state.loginCredentialsForm} />
                                <br/>
                            </ErrorBoundary>
                        </div>
                    </div>

                    <div className="card">
                        <div className={`card-header ${styles.card}`}>
                            Sign up
                        </div>
                        <div className="card-body">
                            <ErrorBoundary error={this.state.createUserApiError}>
                                <Signup
                                    onSubmit={this.handleSignup}
                                    onValidate={this.validateSignup}
                                    initialValues={this.state.createUserForm}
                                    userWasCreated={this.state.userWasCreated}
                                    apiError={this.state.createUserApiError}
                                />
                                <br />
                            </ErrorBoundary>
                        </div>
                    </div>
                </div>
            </>
        );
    }    
}

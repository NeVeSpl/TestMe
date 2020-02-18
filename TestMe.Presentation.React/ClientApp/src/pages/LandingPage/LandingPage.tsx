import * as React from 'react';
import { RouteComponentProps } from 'react-router-dom';
import { UserService } from '../../services';
import { TokensService, LoginCredentialsDTO } from '../../autoapi/services/TokensService';


export class LandingPage extends React.Component<RouteComponentProps>
{
    tokensService = new TokensService();


    handleLogin = (e: React.MouseEvent<HTMLElement>) =>
    {
        e.preventDefault();

        const params = new URLSearchParams(this.props.location.search);

        const loginCredentials = new LoginCredentialsDTO();
        loginCredentials.email = params.get("userId") || "test@test.com";
        loginCredentials.password = "123456789";

        this.tokensService.createToken(loginCredentials)
            .then(x =>
            {
                UserService.setToken(x.token);
                this.props.history.push("/testcreation");
            });
    }

    render()
    {
        return (
            <div>
               
                <form>
                    <input type="submit" value="Log in" onClick={this.handleLogin}/>
                </form>
            </div>
        );
    }
}

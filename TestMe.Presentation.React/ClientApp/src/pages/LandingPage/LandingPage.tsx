import * as React from 'react';
import { TokensService, LoginCredentials } from '../../api';
import { RouteComponentProps } from 'react-router-dom';
import { UserService } from '../../services';


export class LandingPage extends React.Component<RouteComponentProps>
{
    tokensService = new TokensService();


    handleLogin = (e: React.MouseEvent<HTMLElement>) =>
    {
        e.preventDefault();

        const params = new URLSearchParams(this.props.location.search);

        const loginCredentials = new LoginCredentials();
        loginCredentials.login = params.get("userId") || "User 1";
        loginCredentials.password = "whatever";

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

import { ApiBaseService } from "./base";

export class TokensService extends ApiBaseService 
{
    createToken(loginCredentials: LoginCredentials): Promise<CreateTokenResponse>
    {
        return this.MakeRequest<CreateTokenResponse>("POST", "Tokens", loginCredentials);
    }
}


export interface CreateTokenResponse
{
    token: string;
}
export class LoginCredentials
{
    login: string;
    password: string;

    constructor()
    {
        this.login = "";
        this.password = "";
    }
}

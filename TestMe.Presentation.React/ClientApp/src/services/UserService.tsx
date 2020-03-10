import jwt_decode from "jwt-decode";
import { StateStorage } from "../utils";

export class UserService
{
    static storage = new StateStorage<User>("User");


    static isAuthenticated(): boolean
    {
        const user = UserService.storage.Load();
        const isAuthenticated = user != null;
        return isAuthenticated;
    }

    static setToken(token : string)
    {
        const user = new User();
        user.token = token;
        user.id = UserService.parseUserID(token);
        UserService.storage.Save(user);
    }

    static getUserID() : number
    {       
        const user = UserService.storage.Load();
        return user?.id ?? - 1;
    }

    static getUserToken(): string
    {
        const user = UserService.storage.Load();
        return user?.token ??  ">:-o";
    }

    static parseUserID(token: string): number
    {
        if (token != null)
        {
            const obj = jwt_decode(token) as any;
            return obj["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];
        }
        return -1;
    }
}

class User
{
    id?: number;
    token?: string;
}
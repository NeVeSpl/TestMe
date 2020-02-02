import jwt_decode from "jwt-decode";

export class UserService
{
    static isAuthenticated(): boolean
    {
        const token = localStorage.getItem("token");
        const isAuthenticated = token != null;
        return isAuthenticated;
    }

    static setToken(token : string)
    {
        localStorage.setItem('token', token);
    }

    static getUserID() : string
    {
        const token = localStorage.getItem("token");
        if (token != null)
        {
            const obj = jwt_decode(token) as any;
            return obj["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];
        } 
        return "";
    }
}
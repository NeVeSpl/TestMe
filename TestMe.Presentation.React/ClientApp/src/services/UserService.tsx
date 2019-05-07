
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
}
﻿// This file was autogenerated by http://frhagn.github.io/Typewriter/


//eslint-disable-next-line
import { ApiBaseService, IUseRequest, IUseRequestWithResult, useRequest, useRequestWithResult, CursorPagedResults, CursorPagination, OffsetPagedResults, OffsetPagination } from "../base/index";
import { CreateUserDTO } from "../dtos/TestMe.Presentation.API.Controllers.Users.Input.CreateUserDTO";
import { UserDTO } from "../dtos/TestMe.UserManagement.App.Users.Output.UserDTO";
export * from "../dtos/TestMe.Presentation.API.Controllers.Users.Input.CreateUserDTO";
export * from "../dtos/TestMe.UserManagement.App.Users.Output.UserDTO";
export * from "../base/index";

export class UsersService extends ApiBaseService
{
    static Type = "UsersService";

    createUser(createUser: CreateUserDTO) : Promise<number>
    {
        return this.MakeRequestWithResult<number>("post", `Users`, createUser);
    }
    readUsers(pagination: CursorPagination) : Promise<CursorPagedResults<UserDTO>>
    {
        return this.MakeRequestWithResult<CursorPagedResults<UserDTO>>("get", `Users?cursor=${pagination.cursor}&fetchNext=${pagination.fetchNext}`, null);
    }
    isEmailAddressTaken(emailAddress: string) : Promise<boolean>
    {
        return this.MakeRequestWithResult<boolean>("get", `Users/EmailAddress/IsTaken?emailAddress=${encodeURIComponent(emailAddress)}`, null);
    }
    
}

export function useAPICreateUser(createUser: CreateUserDTO, deps?: ReadonlyArray<unknown>) : IUseRequestWithResult<number>
{
    return useRequestWithResult<number>("post", `Users`, createUser, 0,  deps);
}
export function useAPIReadUsers(pagination: CursorPagination, deps?: ReadonlyArray<unknown>) : IUseRequestWithResult<CursorPagedResults<UserDTO>>
{
    return useRequestWithResult<CursorPagedResults<UserDTO>>("get", `Users?cursor=${pagination.cursor}&fetchNext=${pagination.fetchNext}`, null, {} as CursorPagedResults<UserDTO>,  deps);
}
export function useAPIIsEmailAddressTaken(emailAddress: string, deps?: ReadonlyArray<unknown>) : IUseRequestWithResult<boolean>
{
    return useRequestWithResult<boolean>("get", `Users/EmailAddress/IsTaken?emailAddress=${encodeURIComponent(emailAddress)}`, null, false,  deps);
}


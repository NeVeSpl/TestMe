﻿// This file was autogenerated by http://frhagn.github.io/Typewriter/


//eslint-disable-next-line
import { ApiBaseService, IUseRequest, IUseRequestWithResult, useRequest, useRequestWithResult, CursorPagedResults, CursorPagination, OffsetPagedResults, OffsetPagination } from "../base/index";
//eslint-disable-next-line 
 import { CatalogHeaderDTO } from "../dtos/TestMe.TestCreation.App.TestsCatalogs.Output.CatalogHeaderDTO";
//eslint-disable-next-line 
 import { CatalogDTO } from "../dtos/TestMe.TestCreation.App.TestsCatalogs.Output.CatalogDTO";
//eslint-disable-next-line 
 import { CreateCatalogDTO } from "../dtos/TestMe.Presentation.API.Controllers.TestsCatalogs.Input.CreateCatalogDTO";
//eslint-disable-next-line 
 import { UpdateCatalogDTO } from "../dtos/TestMe.Presentation.API.Controllers.TestsCatalogs.Input.UpdateCatalogDTO";
export * from "../dtos/TestMe.TestCreation.App.TestsCatalogs.Output.CatalogHeaderDTO";
export * from "../dtos/TestMe.TestCreation.App.TestsCatalogs.Output.CatalogDTO";
export * from "../dtos/TestMe.Presentation.API.Controllers.TestsCatalogs.Input.CreateCatalogDTO";
export * from "../dtos/TestMe.Presentation.API.Controllers.TestsCatalogs.Input.UpdateCatalogDTO";
export * from "../base/index";

export class TestsCatalogsService extends ApiBaseService
{
    readCatalogHeaders(ownerId: number, pagination: OffsetPagination) : Promise<OffsetPagedResults<CatalogHeaderDTO>>
    {
        return this.MakeRequestWithResult<OffsetPagedResults<CatalogHeaderDTO>>("get", `TestsCatalogs/headers?ownerId=${ownerId}&offset=${pagination.offset}&limit=${pagination.limit}`, null);
    }
    readTestsCatalog(catalogId: number) : Promise<CatalogDTO>
    {
        return this.MakeRequestWithResult<CatalogDTO>("get", `TestsCatalogs/${catalogId}`, null);
    }
    createCatalog(createCatalog: CreateCatalogDTO) : Promise<number>
    {
        return this.MakeRequestWithResult<number>("post", `TestsCatalogs`, createCatalog);
    }
    updateCatalog(catalogId: number, updateCatalog: UpdateCatalogDTO) 
    {
        return this.MakeRequest("put", `TestsCatalogs/${catalogId}`, updateCatalog);
    }
    deleteCatalog(catalogId: number) 
    {
        return this.MakeRequest("delete", `TestsCatalogs/${catalogId}`, null);
    }
           
}

export function useAPIReadCatalogHeaders(ownerId: number, pagination: OffsetPagination, deps?: ReadonlyArray<unknown>) : IUseRequestWithResult<OffsetPagedResults<CatalogHeaderDTO>>
{
    return useRequestWithResult<OffsetPagedResults<CatalogHeaderDTO>>("get", `TestsCatalogs/headers?ownerId=${ownerId}&offset=${pagination.offset}&limit=${pagination.limit}`, null, {} as OffsetPagedResults<CatalogHeaderDTO>,  deps);
}
export function useAPIReadTestsCatalog(catalogId: number, deps?: ReadonlyArray<unknown>) : IUseRequestWithResult<CatalogDTO>
{
    return useRequestWithResult<CatalogDTO>("get", `TestsCatalogs/${catalogId}`, null, new CatalogDTO(),  deps);
}
export function useAPICreateCatalog(createCatalog: CreateCatalogDTO, deps?: ReadonlyArray<unknown>) : IUseRequestWithResult<number>
{
    return useRequestWithResult<number>("post", `TestsCatalogs`, createCatalog, 0,  deps);
}
export function useAPIUpdateCatalog(catalogId: number, updateCatalog: UpdateCatalogDTO, deps?: ReadonlyArray<unknown>) : IUseRequest
{
    return useRequest("put", `TestsCatalogs/${catalogId}`, updateCatalog, deps);
}
export function useAPIDeleteCatalog(catalogId: number, deps?: ReadonlyArray<unknown>) : IUseRequest
{
    return useRequest("delete", `TestsCatalogs/${catalogId}`, null, deps);
}


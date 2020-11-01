﻿// This file was autogenerated by http://frhagn.github.io/Typewriter/


//eslint-disable-next-line
import { ApiBaseService, IUseRequest, IUseRequestWithResult, useRequest, useRequestWithResult, CursorPagedResults, CursorPagination, OffsetPagedResults, OffsetPagination } from "../base/index";
//eslint-disable-next-line 
 import { CatalogOnListDTO } from "../dtos/TestMe.TestCreation.App.RequestHandlers.QuestionsCatalogs.ReadCatalogs.CatalogOnListDTO";
//eslint-disable-next-line 
 import { CatalogDTO } from "../dtos/TestMe.TestCreation.App.RequestHandlers.QuestionsCatalogs.ReadCatalog.CatalogDTO";
//eslint-disable-next-line 
 import { CreateCatalogDTO } from "../dtos/TestMe.Presentation.API.Controllers.QuestionsCatalogs.Input.CreateCatalogDTO";
//eslint-disable-next-line 
 import { UpdateCatalogDTO } from "../dtos/TestMe.Presentation.API.Controllers.QuestionsCatalogs.Input.UpdateCatalogDTO";
export * from "../dtos/TestMe.TestCreation.App.RequestHandlers.QuestionsCatalogs.ReadCatalogs.CatalogOnListDTO";
export * from "../dtos/TestMe.TestCreation.App.RequestHandlers.QuestionsCatalogs.ReadCatalog.CatalogDTO";
export * from "../dtos/TestMe.Presentation.API.Controllers.QuestionsCatalogs.Input.CreateCatalogDTO";
export * from "../dtos/TestMe.Presentation.API.Controllers.QuestionsCatalogs.Input.UpdateCatalogDTO";
export * from "../base/index";

export class QuestionsCatalogsService extends ApiBaseService
{
    static Type = "QuestionsCatalogsService";

    readQuestionsCatalogs(ownerId: number, pagination: OffsetPagination) : Promise<OffsetPagedResults<CatalogOnListDTO>>
    {
        return this.MakeRequestWithResult<OffsetPagedResults<CatalogOnListDTO>>("get", `QuestionsCatalogs?ownerId=${ownerId}&offset=${pagination.offset}&limit=${pagination.limit}`, null);
    }
    readQuestionsCatalog(catalogId: number) : Promise<CatalogDTO>
    {
        return this.MakeRequestWithResult<CatalogDTO>("get", `QuestionsCatalogs/${catalogId}`, null);
    }
    createCatalog(createCatalog: CreateCatalogDTO) : Promise<number>
    {
        return this.MakeRequestWithResult<number>("post", `QuestionsCatalogs`, createCatalog);
    }
    updateCatalog(catalogId: number, updateCatalog: UpdateCatalogDTO) 
    {
        return this.MakeRequest("put", `QuestionsCatalogs/${catalogId}`, updateCatalog);
    }
    deleteCatalog(catalogId: number) 
    {
        return this.MakeRequest("delete", `QuestionsCatalogs/${catalogId}`, null);
    }
           
}

export function useAPIReadQuestionsCatalogs(ownerId: number, pagination: OffsetPagination, deps?: ReadonlyArray<unknown>) : IUseRequestWithResult<OffsetPagedResults<CatalogOnListDTO>>
{
    return useRequestWithResult<OffsetPagedResults<CatalogOnListDTO>>("get", `QuestionsCatalogs?ownerId=${ownerId}&offset=${pagination.offset}&limit=${pagination.limit}`, null, {} as OffsetPagedResults<CatalogOnListDTO>,  deps);
}
export function useAPIReadQuestionsCatalog(catalogId: number, deps?: ReadonlyArray<unknown>) : IUseRequestWithResult<CatalogDTO>
{
    return useRequestWithResult<CatalogDTO>("get", `QuestionsCatalogs/${catalogId}`, null, new CatalogDTO(),  deps);
}
export function useAPICreateCatalog(createCatalog: CreateCatalogDTO, deps?: ReadonlyArray<unknown>) : IUseRequestWithResult<number>
{
    return useRequestWithResult<number>("post", `QuestionsCatalogs`, createCatalog, 0,  deps);
}
export function useAPIUpdateCatalog(catalogId: number, updateCatalog: UpdateCatalogDTO, deps?: ReadonlyArray<unknown>) : IUseRequest
{
    return useRequest("put", `QuestionsCatalogs/${catalogId}`, updateCatalog, deps);
}
export function useAPIDeleteCatalog(catalogId: number, deps?: ReadonlyArray<unknown>) : IUseRequest
{
    return useRequest("delete", `QuestionsCatalogs/${catalogId}`, null, deps);
}


﻿// This file was autogenerated by https://github.com/NeVeSpl/NTypewriter


//eslint-disable-next-line
import { ApiBaseService, IUseRequest, IUseRequestWithResult, useRequest, useRequestWithResult, CursorPagedResults, CursorPagination, OffsetPagedResults, OffsetPagination } from "../base/index";
import { CatalogOnListDTO } from "../dtos/TestMe.TestCreation.App.RequestHandlers.QuestionsCatalogs.ReadCatalogs.CatalogOnListDTO";
import { CatalogDTO } from "../dtos/TestMe.TestCreation.App.RequestHandlers.QuestionsCatalogs.ReadCatalog.CatalogDTO";
import { CreateCatalogDTO } from "../dtos/TestMe.Presentation.API.Controllers.QuestionsCatalogs.Input.CreateCatalogDTO";
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



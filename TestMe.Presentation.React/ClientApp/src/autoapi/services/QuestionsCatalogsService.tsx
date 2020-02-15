﻿// This file was autogenerated by http://frhagn.github.io/Typewriter/


import { ApiBaseService } from "../base/index";
import { OffsetPagedResults } from "../base/OffsetPagedResults";
import { CatalogHeaderDTO } from "../dtos/TestMe.TestCreation.App.QuestionsCatalogs.Output.CatalogHeaderDTO";
import { OffsetPagination } from "../base/OffsetPagination";
import { QuestionsCatalogDTO } from "../dtos/TestMe.TestCreation.App.QuestionsCatalogs.Output.QuestionsCatalogDTO";
import { CatalogDTO } from "../dtos/TestMe.TestCreation.App.QuestionsCatalogs.Output.CatalogDTO";
import { CreateCatalogDTO } from "../dtos/TestMe.Presentation.API.Controllers.QuestionsCatalogs.Input.CreateCatalogDTO";
import { UpdateCatalogDTO } from "../dtos/TestMe.Presentation.API.Controllers.QuestionsCatalogs.Input.UpdateCatalogDTO";
export * from "../base/OffsetPagedResults";
export * from "../dtos/TestMe.TestCreation.App.QuestionsCatalogs.Output.CatalogHeaderDTO";
export * from "../base/OffsetPagination";
export * from "../dtos/TestMe.TestCreation.App.QuestionsCatalogs.Output.QuestionsCatalogDTO";
export * from "../dtos/TestMe.TestCreation.App.QuestionsCatalogs.Output.CatalogDTO";
export * from "../dtos/TestMe.Presentation.API.Controllers.QuestionsCatalogs.Input.CreateCatalogDTO";
export * from "../dtos/TestMe.Presentation.API.Controllers.QuestionsCatalogs.Input.UpdateCatalogDTO";
export * from "../base/index";

export class QuestionsCatalogsService extends ApiBaseService
{
    readQuestionsCatalogHeaders(ownerId: number, pagination: OffsetPagination) : Promise<OffsetPagedResults<CatalogHeaderDTO>>
    {
        return this.MakeRequest<OffsetPagedResults<CatalogHeaderDTO>>("get", `QuestionsCatalogs/headers?ownerId=${ownerId}&offset=${pagination.offset}&limit=${pagination.limit}`, null);
    }
    readQuestionsCatalogHeader(catalogId: number) : Promise<CatalogHeaderDTO>
    {
        return this.MakeRequest<CatalogHeaderDTO>("get", `QuestionsCatalogs/${catalogId}/header`, null);
    }
    readQuestionsCatalog(catalogId: number) : Promise<QuestionsCatalogDTO>
    {
        return this.MakeRequest<QuestionsCatalogDTO>("get", `QuestionsCatalogs/${catalogId}`, null);
    }
    createCatalog(createCatalog: CreateCatalogDTO) : Promise<number>
    {
        return this.MakeRequest<number>("post", `QuestionsCatalogs`, createCatalog);
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
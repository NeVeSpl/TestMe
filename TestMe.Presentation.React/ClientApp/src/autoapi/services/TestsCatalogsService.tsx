﻿// This file was autogenerated by http://frhagn.github.io/Typewriter/


import { ApiBaseService } from "../base/index";
import { OffsetPagedResults } from "../base/OffsetPagedResults";
import { CatalogHeaderDTO } from "../dtos/TestMe.TestCreation.App.TestsCatalogs.Output.CatalogHeaderDTO";
import { OffsetPagination } from "../base/OffsetPagination";
import { CatalogDTO } from "../dtos/TestMe.TestCreation.App.TestsCatalogs.Output.CatalogDTO";
import { CreateCatalogDTO } from "../dtos/TestMe.Presentation.API.Controllers.TestsCatalogs.Input.CreateCatalogDTO";
import { UpdateCatalogDTO } from "../dtos/TestMe.Presentation.API.Controllers.TestsCatalogs.Input.UpdateCatalogDTO";
export * from "../base/OffsetPagedResults";
export * from "../dtos/TestMe.TestCreation.App.TestsCatalogs.Output.CatalogHeaderDTO";
export * from "../base/OffsetPagination";
export * from "../dtos/TestMe.TestCreation.App.TestsCatalogs.Output.CatalogDTO";
export * from "../dtos/TestMe.Presentation.API.Controllers.TestsCatalogs.Input.CreateCatalogDTO";
export * from "../dtos/TestMe.Presentation.API.Controllers.TestsCatalogs.Input.UpdateCatalogDTO";
export * from "../base/index";

export class TestsCatalogsService extends ApiBaseService
{
    readCatalogHeaders(ownerId: number, pagination: OffsetPagination) : Promise<OffsetPagedResults<CatalogHeaderDTO>>
    {
        return this.MakeRequest<OffsetPagedResults<CatalogHeaderDTO>>("get", `TestsCatalogs/headers?ownerId=${ownerId}&offset=${pagination.offset}&limit=${pagination.limit}`, null);
    }
    readTestsCatalog(catalogId: number) : Promise<CatalogDTO>
    {
        return this.MakeRequest<CatalogDTO>("get", `TestsCatalogs/${catalogId}`, null);
    }
    createCatalog(createCatalog: CreateCatalogDTO) : Promise<number>
    {
        return this.MakeRequest<number>("post", `TestsCatalogs`, createCatalog);
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
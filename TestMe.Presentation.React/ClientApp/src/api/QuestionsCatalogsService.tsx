import { ApiBaseService } from "./base";

export class QuestionsCatalogsService extends ApiBaseService 
{
    ReadQuestionsCatalogHeaders(ownerId: string): Promise<CatalogHeader[]>
    {
        return this.MakeRequest<CatalogHeader[]>("GET", `QuestionsCatalogs/headers?ownerId=${ownerId}`);
    }

    ReadQuestionsCatalogHeader(catalogId: number): Promise<CatalogHeader>
    {
        return this.MakeRequest<CatalogHeader>("GET", `QuestionsCatalogs/${catalogId}/header`);
    }

    ReadQuestionsCatalog(catalogId: number): Promise<Catalog>
    {
        return this.MakeRequest<Catalog>("GET", `QuestionsCatalogs/${catalogId}`);
    }

    CreateCatalog(createCatalog: CreateCatalog): Promise<number>
    {   
        return this.MakeRequest<number>("POST", "QuestionsCatalogs", createCatalog);
    }

    DeleteCatalog(catalogId : number)
    {
        return this.MakeRequest("DELETE", `QuestionsCatalogs/${catalogId}`)
    }

    UpdateCatalog(catalogId: number, updateCatalog: UpdateCatalog)
    {
        return this.MakeRequest("PUT", `QuestionsCatalogs/${catalogId}`, updateCatalog)
    }
}

export class CreateCatalog
{
    public name: string;

    constructor()
    {
        this.name = "";
    }
}
export class UpdateCatalog extends CreateCatalog
{
    
}

export class CatalogHeader 
{
    catalogId: number;
    name: string;


    constructor()
    {
        this.catalogId = 0;
        this.name = "";
    }
}
export class Catalog extends CatalogHeader
{
    
}
import { ApiBaseService } from "./base";

export class TestsCatalogsService extends ApiBaseService 
{
    ReadCatalogHeaders(): Promise<TestsCatalogHeader[]>
    {
        return this.MakeRequest<TestsCatalogHeader[]>("GET", "TestsCatalogs/headers");
    }

    ReadCatalog(catalogId: number): Promise<TestsCatalog>
    {
        return this.MakeRequest<TestsCatalog>("GET", `TestsCatalogs/${catalogId}`);
    }

    CreateCatalog(createCatalog: CreateTestsCatalog): Promise<number>
    {
        return this.MakeRequest<number>("POST", "TestsCatalogs", createCatalog);
    }

    DeleteCatalog(catalogId: number)
    {
        return this.MakeRequest("DELETE", `TestsCatalogs/${catalogId}`)
    }

    UpdateCatalog(catalogId: number, updateCatalog: UpdateTestsCatalog)
    {
        return this.MakeRequest("PUT", `TestsCatalogs/${catalogId}`, updateCatalog)
    }
}


export class CreateTestsCatalog
{
    public Name: string | undefined;

    constructor()
    {
        this.Name = "";
    }
}

export class UpdateTestsCatalog extends CreateTestsCatalog
{
   
}

export class TestsCatalogHeader 
{
    catalogId: number;
    name: string;


    constructor()
    {
        this.catalogId = 0;
        this.name = "";
    }
}

export class TestsCatalog extends TestsCatalogHeader
{

}
import { ApiBaseService} from "./base";

export class TestsService extends ApiBaseService 
{
    ReadTestHeaders(catalogId: number): Promise<TestHeader[]>
    {
        return this.MakeRequest<TestHeader[]>("GET", `Tests/headers?catalogId=${catalogId}`);
    }

    ReadTestWithQuestionItemsAndQuestionHeaders(testId: number): Promise<Test>
    {
        return this.MakeRequest<Test>("GET", `Tests/${testId}`);
    }

    CreateTest(createTest: CreateTest): Promise<number>
    {   
        return this.MakeRequest<number>("POST", "Tests", createTest);
    }

    DeleteTest(testId : number)
    {
        return this.MakeRequest("DELETE", `Tests/${testId}`)
    }

    UpdateTest(testId: number, updateCatalog: UpdateTest)
    {
        return this.MakeRequest("PUT", `Tests/${testId}`, updateCatalog)
    }
}


export class CreateTest
{
    public Name: string | undefined;

    constructor(name: string)
    {
        this.Name = name;
    }
}

export class UpdateTest extends CreateTest
{
    constructor(name: string)
    {
        super(name);
    }
}

export class TestHeader 
{
    questionId: number;
    content: string;

    constructor()
    {
        this.questionId = -1;
        this.content = "";
    }
}

export class Test extends TestHeader 
{
    
    concurrencyToken: number;

    constructor()
    {
        super();
        this.concurrencyToken = -1;
       
    }
}


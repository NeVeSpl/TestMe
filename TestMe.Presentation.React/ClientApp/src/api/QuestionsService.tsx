import { ApiBaseService } from "./base";

export class QuestionsService extends ApiBaseService 
{
    ReadQuestionHeaders(catalogId: number): Promise<QuestionHeader[]>
    {
        return this.MakeRequest<QuestionHeader[]>("GET", `Questions/headers?catalogId=${catalogId}`);
    }

    ReadQuestionHeader(questionId: number): Promise<QuestionHeader>
    {
        return this.MakeRequest<QuestionHeader>("GET", `Questions/${questionId}/header`);
    }

    ReadQuestionWithAnswers(questionId: number): Promise<Question>
    {
        return this.MakeRequest<Question>("GET", `Questions/${questionId}`);
    }

    CreateQuestionWithAnswers(createQuestion: CreateQuestion): Promise<number>
    {   
        return this.MakeRequest<number>("POST", "Questions", createQuestion);
    }

    DeleteQuestionWithAnswers(questionId : number)
    {
        return this.MakeRequest("DELETE", `Questions/${questionId}`)
    }

    UpdateQuestionWithAnswers(questionId: number, updateCatalog: UpdateQuestion)
    {
        return this.MakeRequest("PUT", `Questions/${questionId}`, updateCatalog)
    }
}

export class CreateQuestion
{
    content: string;
    catalogId: number | undefined;
    answers: Answer[];

    constructor()
    {
        this.content = "";
        this.answers = [];
    }
}
export class UpdateQuestion extends CreateQuestion
{
    concurrencyToken: number | undefined;
}

export class QuestionHeader 
{
    questionId: number;
    content: string;

    constructor()
    {
        this.questionId = 0;
        this.content = "";
    }
}

export class Question extends QuestionHeader 
{
    answers: Answer[];
    concurrencyToken: number;

    constructor()
    {
        super();
        this.answers = [];
        this.concurrencyToken = 0;
    }
}

export class Answer 
{
    answerId: number;
    content: string;
    isCorrect: boolean;


    constructor()
    {
        this.answerId = 0;
        this.content = "";
        this.isCorrect = false;
    }
}
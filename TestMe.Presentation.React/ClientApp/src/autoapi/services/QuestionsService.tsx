﻿// This file was autogenerated by http://frhagn.github.io/Typewriter/


import { ApiBaseService } from "../base/index";
import { OffsetPagedResults } from "../base/OffsetPagedResults";
import { QuestionHeaderDTO } from "../dtos/TestMe.TestCreation.App.Questions.Output.QuestionHeaderDTO";
import { OffsetPagination } from "../base/OffsetPagination";
import { QuestionDTO } from "../dtos/TestMe.TestCreation.App.Questions.Output.QuestionDTO";
import { AnswerDTO } from "../dtos/TestMe.TestCreation.App.Questions.Output.AnswerDTO";
import { CreateQuestionDTO } from "../dtos/TestMe.Presentation.API.Controllers.Questions.Input.CreateQuestionDTO";
import { UpdateQuestionDTO } from "../dtos/TestMe.Presentation.API.Controllers.Questions.Input.UpdateQuestionDTO";
export * from "../base/OffsetPagedResults";
export * from "../dtos/TestMe.TestCreation.App.Questions.Output.QuestionHeaderDTO";
export * from "../base/OffsetPagination";
export * from "../dtos/TestMe.TestCreation.App.Questions.Output.QuestionDTO";
export * from "../dtos/TestMe.TestCreation.App.Questions.Output.AnswerDTO";
export * from "../dtos/TestMe.Presentation.API.Controllers.Questions.Input.CreateQuestionDTO";
export * from "../dtos/TestMe.Presentation.API.Controllers.Questions.Input.UpdateQuestionDTO";
export * from "../base/index";

export class QuestionsService extends ApiBaseService
{
    readQuestionHeaders(catalogId: number, pagination: OffsetPagination) : Promise<OffsetPagedResults<QuestionHeaderDTO>>
    {
        return this.MakeRequest<OffsetPagedResults<QuestionHeaderDTO>>("get", `Questions/headers?catalogId=${catalogId}&offset=${pagination.offset}&limit=${pagination.limit}`, null);
    }
    readQuestionHeadersAsync(catalogId: number) : Promise<QuestionHeaderDTO[]>
    {
        return this.MakeRequest<QuestionHeaderDTO[]>("get", `Questions/headers/async?catalogId=${catalogId}`, null);
    }
    readQuestionHeader(questionId: number) : Promise<QuestionHeaderDTO>
    {
        return this.MakeRequest<QuestionHeaderDTO>("get", `Questions/${questionId}/header`, null);
    }
    readQuestionWithAnswers(questionId: number) : Promise<QuestionDTO>
    {
        return this.MakeRequest<QuestionDTO>("get", `Questions/${questionId}`, null);
    }
    readQuestionWithAnswersAsync(questionId: number) : Promise<QuestionDTO>
    {
        return this.MakeRequest<QuestionDTO>("get", `Questions/${questionId}/async`, null);
    }
    createQuestionWithAnswers(createQuestion: CreateQuestionDTO) : Promise<number>
    {
        return this.MakeRequest<number>("post", `Questions`, createQuestion);
    }
    updateQuestionWithAnswers(questionId: number, updateQuestion: UpdateQuestionDTO) 
    {
        return this.MakeRequest("put", `Questions/${questionId}`, updateQuestion);
    }
    deleteQuestionWithAnswers(questionId: number) 
    {
        return this.MakeRequest("delete", `Questions/${questionId}`, null);
    }
           
}
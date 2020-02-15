﻿// This file was autogenerated by http://frhagn.github.io/Typewriter/


import { ApiBaseService } from "../base/index";
import { OffsetPagedResults } from "../base/OffsetPagedResults";
import { TestHeaderDTO } from "../dtos/TestMe.TestCreation.App.Tests.Output.TestHeaderDTO";
import { OffsetPagination } from "../base/OffsetPagination";
import { TestDTO } from "../dtos/TestMe.TestCreation.App.Tests.Output.TestDTO";
import { QuestionItemDTO } from "../dtos/TestMe.TestCreation.App.Tests.Output.QuestionItemDTO";
import { QuestionHeaderDTO } from "../dtos/TestMe.TestCreation.App.Tests.Output.QuestionHeaderDTO";
import { CreateTestDTO } from "../dtos/TestMe.Presentation.API.Controllers.Tests.Input.CreateTestDTO";
import { UpdateTestDTO } from "../dtos/TestMe.Presentation.API.Controllers.Tests.Input.UpdateTestDTO";
import { CreateQuestionItemDTO } from "../dtos/TestMe.Presentation.API.Controllers.Tests.Input.CreateQuestionItemDTO";
import { UpdateQuestionItemDTO } from "../dtos/TestMe.Presentation.API.Controllers.Tests.Input.UpdateQuestionItemDTO";
export * from "../base/OffsetPagedResults";
export * from "../dtos/TestMe.TestCreation.App.Tests.Output.TestHeaderDTO";
export * from "../base/OffsetPagination";
export * from "../dtos/TestMe.TestCreation.App.Tests.Output.TestDTO";
export * from "../dtos/TestMe.TestCreation.App.Tests.Output.QuestionItemDTO";
export * from "../dtos/TestMe.TestCreation.App.Tests.Output.QuestionHeaderDTO";
export * from "../dtos/TestMe.Presentation.API.Controllers.Tests.Input.CreateTestDTO";
export * from "../dtos/TestMe.Presentation.API.Controllers.Tests.Input.UpdateTestDTO";
export * from "../dtos/TestMe.Presentation.API.Controllers.Tests.Input.CreateQuestionItemDTO";
export * from "../dtos/TestMe.Presentation.API.Controllers.Tests.Input.UpdateQuestionItemDTO";
export * from "../base/index";

export class TestsService extends ApiBaseService
{
    readTestHeaders(catalogId: number, pagination: OffsetPagination) : Promise<OffsetPagedResults<TestHeaderDTO>>
    {
        return this.MakeRequest<OffsetPagedResults<TestHeaderDTO>>("get", `Tests/headers?catalogId=${catalogId}&offset=${pagination.offset}&limit=${pagination.limit}`, null);
    }
    readTestWithQuestionItemsAndQuestionHeaders(testId: number) : Promise<TestDTO>
    {
        return this.MakeRequest<TestDTO>("get", `Tests/${testId}`, null);
    }
    createTest(createTest: CreateTestDTO) : Promise<number>
    {
        return this.MakeRequest<number>("post", `Tests`, createTest);
    }
    updateTest(testId: number, updateTest: UpdateTestDTO) 
    {
        return this.MakeRequest("put", `Tests/${testId}`, updateTest);
    }
    deleteTest(testId: number) 
    {
        return this.MakeRequest("delete", `Tests/${testId}`, null);
    }
    createQuestionItem(testId: number, createQuestionItem: CreateQuestionItemDTO) : Promise<number>
    {
        return this.MakeRequest<number>("post", `Tests/${testId}/questions`, createQuestionItem);
    }
    updateQuestionItem(testId: number, questionItemId: number, updateQuestionItem: UpdateQuestionItemDTO) 
    {
        return this.MakeRequest("put", `Tests/${testId}/questions/${questionItemId}`, updateQuestionItem);
    }
    deleteQuestionItem(testId: number, questionItemId: number) 
    {
        return this.MakeRequest("delete", `Tests/${testId}/questions/${questionItemId}`, null);
    }
           
}
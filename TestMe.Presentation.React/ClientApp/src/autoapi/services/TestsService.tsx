﻿// This file was autogenerated by http://frhagn.github.io/Typewriter/


//eslint-disable-next-line
import { ApiBaseService, IUseRequest, IUseRequestWithResult, useRequest, useRequestWithResult, CursorPagedResults, CursorPagination, OffsetPagedResults, OffsetPagination } from "../base/index";
import { TestOnListDTO } from "../dtos/TestMe.TestCreation.App.RequestHandlers.Tests.ReadTests.TestOnListDTO";
import { TestDTO } from "../dtos/TestMe.TestCreation.App.RequestHandlers.Tests.ReadTest.TestDTO";
import { CreateTestDTO } from "../dtos/TestMe.Presentation.API.Controllers.Tests.Input.CreateTestDTO";
import { UpdateTestDTO } from "../dtos/TestMe.Presentation.API.Controllers.Tests.Input.UpdateTestDTO";
import { TestItemDTO } from "../dtos/TestMe.TestCreation.App.RequestHandlers.Tests.ReadTestItems.TestItemDTO";
import { CreateTestItemDTO } from "../dtos/TestMe.Presentation.API.Controllers.Tests.Input.CreateTestItemDTO";
import { UpdateTestItemDTO } from "../dtos/TestMe.Presentation.API.Controllers.Tests.Input.UpdateTestItemDTO";
export * from "../dtos/TestMe.TestCreation.App.RequestHandlers.Tests.ReadTests.TestOnListDTO";
export * from "../dtos/TestMe.TestCreation.App.RequestHandlers.Tests.ReadTest.TestDTO";
export * from "../dtos/TestMe.Presentation.API.Controllers.Tests.Input.CreateTestDTO";
export * from "../dtos/TestMe.Presentation.API.Controllers.Tests.Input.UpdateTestDTO";
export * from "../dtos/TestMe.TestCreation.App.RequestHandlers.Tests.ReadTestItems.TestItemDTO";
export * from "../dtos/TestMe.Presentation.API.Controllers.Tests.Input.CreateTestItemDTO";
export * from "../dtos/TestMe.Presentation.API.Controllers.Tests.Input.UpdateTestItemDTO";
export * from "../base/index";

export class TestsService extends ApiBaseService
{
    static Type = "TestsService";

    readTests(ownerId: number, pagination: OffsetPagination) : Promise<OffsetPagedResults<TestOnListDTO>>
    {
        return this.MakeRequestWithResult<OffsetPagedResults<TestOnListDTO>>("get", `Tests?ownerId=${ownerId}&offset=${pagination.offset}&limit=${pagination.limit}`, null);
    }
    readTest(testId: number) : Promise<TestDTO>
    {
        return this.MakeRequestWithResult<TestDTO>("get", `Tests/${testId}`, null);
    }
    createTest(createTest: CreateTestDTO) : Promise<number>
    {
        return this.MakeRequestWithResult<number>("post", `Tests`, createTest);
    }
    updateTest(testId: number, updateTest: UpdateTestDTO)
    {
        return this.MakeRequest("put", `Tests/${testId}`, updateTest);
    }
    deleteTest(testId: number)
    {
        return this.MakeRequest("delete", `Tests/${testId}`, null);
    }
    readTestItems(testId: number) : Promise<TestItemDTO[]>
    {
        return this.MakeRequestWithResult<TestItemDTO[]>("get", `Tests/${testId}/questions`, null);
    }
    createTestItem(testId: number, createTestItem: CreateTestItemDTO) : Promise<number>
    {
        return this.MakeRequestWithResult<number>("post", `Tests/${testId}/questions`, createTestItem);
    }
    updateTestItem(testId: number, questionItemId: number, updateTestItem: UpdateTestItemDTO)
    {
        return this.MakeRequest("put", `Tests/${testId}/questions/${questionItemId}`, updateTestItem);
    }
    deleteTestItem(testId: number, questionItemId: number)
    {
        return this.MakeRequest("delete", `Tests/${testId}/questions/${questionItemId}`, null);
    }
    
}

export function useAPIReadTests(ownerId: number, pagination: OffsetPagination, deps?: ReadonlyArray<unknown>) : IUseRequestWithResult<OffsetPagedResults<TestOnListDTO>>
{
    return useRequestWithResult<OffsetPagedResults<TestOnListDTO>>("get", `Tests?ownerId=${ownerId}&offset=${pagination.offset}&limit=${pagination.limit}`, null, {} as OffsetPagedResults<TestOnListDTO>,  deps);
}
export function useAPIReadTest(testId: number, deps?: ReadonlyArray<unknown>) : IUseRequestWithResult<TestDTO>
{
    return useRequestWithResult<TestDTO>("get", `Tests/${testId}`, null, new TestDTO(),  deps);
}
export function useAPICreateTest(createTest: CreateTestDTO, deps?: ReadonlyArray<unknown>) : IUseRequestWithResult<number>
{
    return useRequestWithResult<number>("post", `Tests`, createTest, 0,  deps);
}
export function useAPIUpdateTest(testId: number, updateTest: UpdateTestDTO, deps?: ReadonlyArray<unknown>) : IUseRequest
{
    return useRequest("put", `Tests/${testId}`, updateTest, deps);
}
export function useAPIDeleteTest(testId: number, deps?: ReadonlyArray<unknown>) : IUseRequest
{
    return useRequest("delete", `Tests/${testId}`, null, deps);
}
export function useAPIReadTestItems(testId: number, deps?: ReadonlyArray<unknown>) : IUseRequestWithResult<TestItemDTO[]>
{
    return useRequestWithResult<TestItemDTO[]>("get", `Tests/${testId}/questions`, null, {} as TestItemDTO[],  deps);
}
export function useAPICreateTestItem(testId: number, createTestItem: CreateTestItemDTO, deps?: ReadonlyArray<unknown>) : IUseRequestWithResult<number>
{
    return useRequestWithResult<number>("post", `Tests/${testId}/questions`, createTestItem, 0,  deps);
}
export function useAPIUpdateTestItem(testId: number, questionItemId: number, updateTestItem: UpdateTestItemDTO, deps?: ReadonlyArray<unknown>) : IUseRequest
{
    return useRequest("put", `Tests/${testId}/questions/${questionItemId}`, updateTestItem, deps);
}
export function useAPIDeleteTestItem(testId: number, questionItemId: number, deps?: ReadonlyArray<unknown>) : IUseRequest
{
    return useRequest("delete", `Tests/${testId}/questions/${questionItemId}`, null, deps);
}


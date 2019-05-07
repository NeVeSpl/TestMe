using System.Collections.Generic;
using TestMe.SharedKernel.App;
using TestMe.TestCreation.App.Tests.Input;
using TestMe.TestCreation.App.Tests.Output;

namespace TestMe.TestCreation.App.Tests
{
    public interface ITestsService
    {
        Result<long> CreateTest(long ownerId, CreateTest createTest);
        Result DeleteTest(long ownerId, long testId);
        Result<List<TestHeaderDTO>> ReadTestHeaders(long ownerId, long catalogId);
        Result<TestDTO> ReadTestWithQuestionItemsAndQuestionHeaders(long ownerId, long testId);
        Result UpdateTest(long ownerId, long testId, UpdateTest updateTest);

        Result<long> CreateQuestionItem(long ownerId, long testId, AddQuestionItem addQuestionItem);       
        Result DeleteQuestionItem(long ownerId, long testId, long questionItemId);     
        Result UpdateQuestionItem(long ownerId, long testId, long questionItemId, UpdateQuestionItem updateQuestionItem);       
    }
}
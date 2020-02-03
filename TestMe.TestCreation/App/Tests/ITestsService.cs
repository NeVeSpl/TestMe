using System.Collections.Generic;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.App.Tests.Input;
using TestMe.TestCreation.App.Tests.Output;

namespace TestMe.TestCreation.App.Tests
{
    public interface ITestsService
    {
        Result<long> CreateTest(CreateTest createTest);
        Result DeleteTest(DeleteTest deleteTest);
        Result UpdateTest(UpdateTest updateTest);


        Result<OffsetPagedResults<TestHeaderDTO>> ReadTestHeaders(long ownerId, long catalogId, OffsetPagination pagination);
        Result<TestDTO> ReadTestWithQuestionItemsAndQuestionHeaders(long ownerId, long testId);
        

        Result<long> CreateQuestionItem(CreateQuestionItem createQuestionItem);       
        Result DeleteQuestionItem(DeleteQuestionItem deleteQuestionItem);     
        Result UpdateQuestionItem(UpdateQuestionItem updateQuestionItem);       
    }
}
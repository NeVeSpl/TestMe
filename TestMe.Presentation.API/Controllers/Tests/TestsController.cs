using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestMe.BuildingBlocks.App;
using TestMe.Presentation.API.Controllers.Tests.Input;
using TestMe.TestCreation.App.RequestHandlers.Tests.DeleteTestItem;
using TestMe.TestCreation.App.RequestHandlers.Tests.DeleteTest;
using TestMe.TestCreation.App.RequestHandlers.Tests.ReadTestItems;
using TestMe.TestCreation.App.RequestHandlers.Tests.ReadTest;
using TestMe.TestCreation.App.RequestHandlers.Tests.ReadTests;

namespace TestMe.Presentation.API.Controllers.Tests
{
    [Route("Tests")] // surprise, you can not use [controller] tag if your controller name is "Tests"
    [Authorize]
    [SetupController]
    [ApiConventionType(typeof(ApiConventions))]
    public class TestsController : Controller
    {
        [HttpGet]
        public async Task<ActionResult<OffsetPagedResults<TestOnListDTO>>> ReadTests(long ownerId, [FromQuery] OffsetPagination pagination)
        {
            var result = await Send(new ReadTestsQuery(ownerId, pagination));
            return ActionResult(result);
        }

        [HttpGet("{testId}")]
        public async Task<ActionResult<TestDTO>> ReadTest(long testId)
        {
            var result = await Send(new ReadTestQuery(testId));
            return ActionResult(result);
        }

        [HttpPost]
        public async Task<ActionResult<long>> CreateTest(CreateTestDTO createTest)
        {
            var result = await Send(createTest.CreateCommand());
            return ActionResult(result);
        }

        [HttpPut("{testId}")]
        public async Task<ActionResult> UpdateTest(long testId, UpdateTestDTO updateTest)
        {
            var result = await Send(updateTest.CreateCommand(testId));
            return ActionResult(result);
        }

        [HttpDelete("{testId}")]
        public async Task<ActionResult> DeleteTest(long testId)
        {
            var result = await Send(new DeleteTestCommand(testId));
            return ActionResult(result);
        }


        [HttpGet("{testId}/questions")]
        public async Task<ActionResult<List<TestItemDTO>>> ReadTestItems(long testId)
        {
            var result = await Send(new ReadTestItemsQuery(testId));
            return ActionResult(result);
        }

        [HttpPost("{testId}/questions")]       
        public async Task<ActionResult<long>> CreateTestItem(long testId, CreateTestItemDTO createTestItem)
        {          
            var result = await Send(createTestItem.CreateCommand(testId));
            return ActionResult(result);
        }

        [HttpPut("{testId}/questions/{questionItemId}")]       
        public async Task<ActionResult> UpdateTestItem(long testId, long questionItemId, UpdateTestItemDTO updateTestItem)
        {          
            var result = await Send(updateTestItem.CreateCommand(testId, questionItemId));
            return ActionResult(result);
        }

        [HttpDelete("{testId}/questions/{questionItemId}")]     
        public async Task<ActionResult> DeleteTestItem(long testId, long questionItemId)
        {            
            var result = await Send(new DeleteTestItemCommand(testId, questionItemId));
            return ActionResult(result);
        }
    }
}
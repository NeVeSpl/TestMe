using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TestMe.Presentation.API.Controllers.Tests.Input;
using TestMe.TestCreation.App.Tests;
using TestMe.TestCreation.App.Tests.Input;
using TestMe.TestCreation.App.Tests.Output;

namespace TestMe.Presentation.API.Controllers.Tests
{
    [Route("Tests")] // surprise, you can not use [controller] tag if your controller name is "Tests"
    [Authorize]
    [SetupController]
    [ApiConventionType(typeof(ApiConventions))]
    public class TestsController : Controller
    {
        private readonly ITestsService service;


        public TestsController(ITestsService service)
        {
            this.service = service;
        }


        [HttpGet("headers")]        
        public ActionResult<List<TestHeaderDTO>> ReadTestHeaders(long catalogId)
        {
            var result = service.ReadTestHeaders(UserId, catalogId);
            return ActionResult(result);
        }

        [HttpGet("{testId}")]       
        public ActionResult<TestDTO> ReadTestWithQuestionItemsAndQuestionHeaders(long testId)
        {           
            var result = service.ReadTestWithQuestionItemsAndQuestionHeaders(UserId, testId);
            return ActionResult(result);
        }

        [HttpPost]       
        public ActionResult<long> CreateTest(CreateTestDTO createTest)
        {
            var result = service.CreateTest(createTest.CreateCommand(UserId));
            return ActionResult(result);
        }

        [HttpPut("{testId}")]       
        public ActionResult UpdateTest(long testId, UpdateTestDTO updateTest)
        {            
            var result = service.UpdateTest(updateTest.CreateCommand(UserId, testId));
            return ActionResult(result);
        }

        [HttpDelete("{testId}")]      
        public ActionResult DeleteTest(long testId)
        {
            var result = service.DeleteTest(new DeleteTest(UserId, testId));
            return ActionResult(result);
        }



        [HttpPost("{testId}/questions")]       
        public ActionResult<long> CreateQuestionItem(long testId, CreateQuestionItemDTO createQuestionItem)
        {          
            var result = service.CreateQuestionItem(createQuestionItem.CreateCommand(UserId, testId));
            return ActionResult(result);
        }

        [HttpPut("{testId}/questions/{questionItemId}")]       
        public ActionResult UpdateQuestionItem(long testId, long questionItemId, UpdateQuestionItemDTO updateQuestionItem)
        {          
            var result = service.UpdateQuestionItem(updateQuestionItem.CreateCommand(UserId, testId, questionItemId));
            return ActionResult(result);
        }

        [HttpDelete("{testId}/questions/{questionItemId}")]     
        public ActionResult DeleteQuestionItem(long testId, long questionItemId)
        {
            var result = service.DeleteQuestionItem(new DeleteQuestionItem(UserId, testId, questionItemId));
            return ActionResult(result);
        }
    }
}
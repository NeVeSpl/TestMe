using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestMe.TestCreation.App.Tests;
using TestMe.TestCreation.App.Tests.Input;
using TestMe.TestCreation.App.Tests.Output;

namespace TestMe.Presentation.API.Controllers
{
    [ApiController]
    [Route("tests")]
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
        public ActionResult<long> CreateTest(CreateTest createTest)
        {
            var result = service.CreateTest(UserId, createTest);

            return ActionResult(result);
        }

        [HttpPut("{testId}")]       
        public ActionResult UpdateTest(long testId, UpdateTest updateTest)
        {            
            var result = service.UpdateTest(UserId, testId, updateTest);

            return ActionResult(result);
        }

        [HttpDelete("{testId}")]      
        public ActionResult DeleteTest(long testId)
        {
            var result = service.DeleteTest(UserId, testId);

            return ActionResult(result);
        }


        [HttpPost("{testId}/questions")]       
        public ActionResult<long> CreateQuestionItem(long testId, AddQuestionItem addQuestionItem)
        {          
            var result = service.CreateQuestionItem(UserId, testId, addQuestionItem);

            return ActionResult(result);
        }

        [HttpPut("{testId}/questions/{quetionItemId}")]       
        public ActionResult UpdateQuestionItem(long testId, long quetionItemId, UpdateQuestionItem updateQuestionItem)
        {          
            var result = service.UpdateQuestionItem(UserId, testId, quetionItemId, updateQuestionItem);

            return ActionResult(result);
        }

        [HttpDelete("{testId}/questions/{quetionItemId}")]     
        public ActionResult DeleteQuestionItem(long testId, long quetionItemId)
        {
            var result = service.DeleteQuestionItem(UserId, testId, quetionItemId);

            return ActionResult(result);
        }
    }
}

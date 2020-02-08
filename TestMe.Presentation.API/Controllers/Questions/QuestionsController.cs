using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestMe.BuildingBlocks.App;
using TestMe.Presentation.API.Attributes;
using TestMe.Presentation.API.Controllers.Questions.Input;
using TestMe.TestCreation.App.Questions;
using TestMe.TestCreation.App.Questions.Input;
using TestMe.TestCreation.App.Questions.Output;

namespace TestMe.Presentation.API.Controllers.Questions
{
    [Route("[controller]")]
    [Authorize]
    [SetupController]
    [ApiConventionType(typeof(ApiConventions))]
    public class QuestionsController : Controller
    {
        private readonly IQuestionsService service;


        public QuestionsController(IQuestionsService service)
        {
            this.service = service;
        }


        [HttpGet("headers")]       
        public ActionResult<OffsetPagedResults<QuestionHeaderDTO>> ReadQuestionHeaders(long catalogId, [FromQuery]OffsetPagination pagination)
        {
            var result = service.ReadQuestionHeaders(UserId, catalogId, pagination);
            return ActionResult(result);
        }
        [HttpGet("headers/async")]
        public async Task<ActionResult<List<QuestionHeaderDTO>>> ReadQuestionHeadersAsync(long catalogId)
        {
            var result = await service.ReadQuestionHeadersAsync(UserId, catalogId);
            return ActionResult(result);
        }
        [HttpGet("{questionId}/header")]
        public ActionResult<QuestionHeaderDTO> ReadQuestionHeader(long questionId)
        {
            var result = service.ReadQuestionHeader(UserId, questionId);
            return ActionResult(result);
        }

        [HttpGet("{questionId}")]  
        [AddETagFromConcurrencyToken]
        public ActionResult<QuestionDTO> ReadQuestionWithAnswers(long questionId)
        {
            var result = service.ReadQuestionWithAnswers(UserId, questionId);           
            return ActionResult(result);
        }
        [HttpGet("{questionId}/async")]
        [AddETagFromConcurrencyToken]
        public async Task<ActionResult<QuestionDTO>> ReadQuestionWithAnswersAsync(long questionId)
        {
            var result = await service.ReadQuestionWithAnswersAsync(UserId, questionId);
            return ActionResult(result);
        }

        [HttpPost]       
        public ActionResult<long> CreateQuestionWithAnswers(CreateQuestionDTO createQuestion)
        {
            var result = Execute(service.CreateQuestionWithAnswers, createQuestion.CreateCommand());
            return ActionResult(result);
        }
       
        [HttpPut("{questionId}")]
        [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.UpdateWithConcurrencyCheck))]       
        public ActionResult UpdateQuestionWithAnswers(long questionId, UpdateQuestionDTO updateQuestion)
        {         
            var result = Execute(service.UpdateQuestionWithAnswers, updateQuestion.CreateCommand(questionId));
            return ActionResult(result);
        }
      
        [HttpDelete("{questionId}")]      
        public ActionResult DeleteQuestionWithAnswers(long questionId)
        {
            var result = Execute(service.DeleteQuestionWithAnswers, new DeleteQuestion(questionId));
            return ActionResult(result);
        }
    }
}
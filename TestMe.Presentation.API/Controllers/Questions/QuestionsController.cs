using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestMe.BuildingBlocks.App;
using TestMe.Presentation.API.Attributes;
using TestMe.Presentation.API.Controllers.Questions.Input;
using TestMe.TestCreation.App.RequestHandlers.Questions.DeleteQuestion;
using TestMe.TestCreation.App.RequestHandlers.Questions.ReadQuestion;
using TestMe.TestCreation.App.RequestHandlers.Questions.ReadQuestions;

namespace TestMe.Presentation.API.Controllers.Questions
{
    [Route("[controller]")]
    [Authorize]
    [SetupController]
    [ApiConventionType(typeof(ApiConventions))]
    public class QuestionsController : Controller
    {
        [HttpGet]       
        public async Task<ActionResult<OffsetPagedResults<QuestionOnListDTO>>> ReadQuestions(long catalogId, [FromQuery]OffsetPagination pagination)
        {            
            var result = await Send(new ReadQuestionsQuery(catalogId, pagination));
            return ActionResult(result);
        }

        [HttpGet("{questionId}")]
        [AddETagFromConcurrencyToken]
        public async Task<ActionResult<QuestionWithAnswersDTO>> ReadQuestionWithAnswers(long questionId)
        {
            var result = await Send(new ReadQuestionWithAnswersQuery(questionId));
            return ActionResult(result);
        }

        [HttpPost]
        public async Task<ActionResult<long>> CreateQuestionWithAnswers(CreateQuestionDTO createQuestion)
        {
            var result = await Send(createQuestion.CreateCommand());
            return ActionResult(result);
        }        
       
        [HttpPut("{questionId}")]
        [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.UpdateWithConcurrencyCheck))]       
        public async Task<ActionResult> UpdateQuestionWithAnswers(long questionId, UpdateQuestionDTO updateQuestion)
        {         
            var result = await Send(updateQuestion.CreateCommand(questionId));

            if (result.Status == ResultStatus.Conflict)
            {
                var newQuestionVersion = await Send(new ReadQuestionWithAnswersQuery(questionId));
                return Conflict(newQuestionVersion.Value);
            }

            return ActionResult(result);
        }
      
        [HttpDelete("{questionId}")]      
        public async Task<ActionResult> DeleteQuestionWithAnswers(long questionId)
        {
            var result = await Send(new DeleteQuestionWithAnswersCommand(questionId));
            return ActionResult(result);
        }
    }
}
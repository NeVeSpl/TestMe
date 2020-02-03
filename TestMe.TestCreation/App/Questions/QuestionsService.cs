using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.App.Questions.Input;
using TestMe.TestCreation.App.Questions.Output;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.App.Questions
{
    internal sealed class QuestionsService : IQuestionsService
    {
        private readonly QuestionReader questionReader;
        private readonly ITestCreationUoW uow;


        public QuestionsService(QuestionReader questionReader, ITestCreationUoW uow)
        {
            this.questionReader = questionReader;
            this.uow = uow;
        }


        public Result<OffsetPagedResults<QuestionHeaderDTO>> ReadQuestionHeaders(long ownerId, long catalogId, OffsetPagination pagination)
        { 
            return questionReader.GetQuestionsHeaders(ownerId, catalogId, pagination);
        }
        public async Task<Result<List<QuestionHeaderDTO>>> ReadQuestionHeadersAsync(long ownerId, long catalogId)
        {
            return await questionReader.GetQuestionsHeadersAsync(ownerId, catalogId);
        }

        public Result<QuestionHeaderDTO> ReadQuestionHeader(long ownerId, long questionId)
        {           
            return questionReader.GetQuestionHeader(ownerId, questionId);
        }

        public Result<QuestionDTO> ReadQuestionWithAnswers(long ownerId, long questionId)
        {            
            return questionReader.GetQuestion(ownerId, questionId, true);
        }
        public async Task<Result<QuestionDTO>> ReadQuestionWithAnswersAsync(long ownerId, long questionId)
        {
            return await questionReader.GetQuestionAsync(ownerId, questionId, true);
        }

        public Result<long> CreateQuestionWithAnswers(CreateQuestion createQuestion)
        {
            var catalog = uow.QuestionsCatalogs.GetById(createQuestion.CatalogId);

            if (catalog == null)
            {
                return Result.Error("Catalog not found");
            }
            if (catalog.OwnerId != createQuestion.UserId)
            {
                return Result.Unauthorized();
            }

            var question = Question.Create(createQuestion.Content, createQuestion.UserId);

            if (createQuestion.Answers != null)
            {
                foreach (var answer in createQuestion.Answers)
                {
                    question.AddAnswer(answer.Content, answer.IsCorrect);
                }
            }

            var owner = uow.Owners.GetById(catalog.OwnerId);
            var policy = AddQuestionPolicyFactory.Create(owner.MembershipLevel);
            catalog.AddQuestion(question, policy);
            uow.Save();

            return Result.Ok(question.QuestionId);
        }

        public Result UpdateQuestionWithAnswers(UpdateQuestion updateQuestion)
        {
            Question question = uow.Questions.GetByIdWithAnswers(updateQuestion.QuestionId);

            if (question == null)
            {
                return Result.NotFound();
            }
            if (question.OwnerId != updateQuestion.UserId) // todo : check catalog instead
            {
                return Result.Unauthorized();
            }
            if (updateQuestion.ConcurrencyToken.HasValue)
            {
                if (question.ConcurrencyToken != updateQuestion.ConcurrencyToken.Value)
                {
                    return Result.Conflict();
                }
            }

            question.Content = updateQuestion.Content;

            if (question.CatalogId != updateQuestion.CatalogId)
            {
                var owner = uow.Owners.GetById(question.OwnerId);
                var policy = AddQuestionPolicyFactory.Create(owner.MembershipLevel);
                QuestionMover.MoveQuestionToCatalog(question, updateQuestion.CatalogId, uow.QuestionsCatalogs, policy);
            }

            question.Answers.MergeWith(updateQuestion.Answers, x => x.AnswerId, y => y.AnswerId,
                                       onAdd:     x => question.AddAnswer(x.Content, x.IsCorrect), 
                                       onUpdate:  (x, y) => { x.Content = y.Content; x.IsCorrect = y.IsCorrect; },
                                       onDelete:  y => question.DeleteAnswer(y));
           
            uow.Save();                     
            
            return Result.Ok();
        }

        public Result DeleteQuestionWithAnswers(DeleteQuestion deleteQuestion)
        {
            var question = uow.Questions.GetByIdWithAnswers(deleteQuestion.QuestionId);

            if (question == null)
            {
                return Result.NotFound();
            }

            var catalog = uow.QuestionsCatalogs.GetById(question.CatalogId);

            if (catalog.OwnerId != deleteQuestion.UserId)
            {
                return Result.Unauthorized();
            }

            catalog.DeleteQuestion(question);           
            uow.Save();

            return Result.Ok();
        }
    }
}
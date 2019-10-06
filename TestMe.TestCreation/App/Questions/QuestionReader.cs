using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestMe.SharedKernel.App;
using TestMe.TestCreation.App.Questions.Output;
using TestMe.TestCreation.Domain;
using TestMe.TestCreation.Persistence;

namespace TestMe.TestCreation.App.Questions
{
    internal sealed class QuestionReader
    { 
        private readonly ReadOnlyTestCreationDbContext context;


        public QuestionReader(ReadOnlyTestCreationDbContext context)
        {
            this.context = context;            
        }


        public Result<QuestionDTO> GetQuestion(long ownerId, long questionId, bool includeAnswers)
        { 
            Question question = context.Questions.Where(x => x.QuestionId == questionId).FirstOrDefault();

            if (question == null)
            {
                return Result.NotFound();
            }

            var catalog = context.Questions.Where(x => x.QuestionId == questionId).Join(context.QuestionsCatalogs, 
                                                                                         x => x.CatalogId,
                                                                                         x => x.CatalogId,
                                                                                         (x,y) =>  new { x.OwnerId }).FirstOrDefault();

            if (catalog.OwnerId != ownerId)
            {
                return Result.Unauthorized();
            }

            QuestionDTO dto = QuestionDTO.Mapping(question);

            return Result.Ok(dto);
        }
        public async Task<Result<QuestionDTO>> GetQuestionAsync(long ownerId, long questionId, bool includeAnswers)
        {
            Question question = await context.Questions.Where(x => x.QuestionId == questionId).FirstOrDefaultAsync();

            if (question == null)
            {
                return Result.NotFound();
            }

            var catalog = await context.Questions.Where(x => x.QuestionId == questionId).Join(context.QuestionsCatalogs,
                                                                                         x => x.CatalogId,
                                                                                         x => x.CatalogId,
                                                                                         (x, y) => new { x.OwnerId }).FirstOrDefaultAsync();

            if (catalog.OwnerId != ownerId)
            {
                return Result.Unauthorized();
            }

            QuestionDTO dto = QuestionDTO.Mapping(question);

            return Result.Ok(dto);
        }

        public Result<QuestionHeaderDTO> GetQuestionHeader(long ownerId, long questionId)
        {
            var question = context.Questions.Where(x => x.QuestionId == questionId).Select(QuestionHeaderDTO.MappingExpr).FirstOrDefault();

            if (question == null)
            {
                return Result.NotFound();
            }

            var catalog = context.Questions.Where(x => x.QuestionId == questionId).Join(context.QuestionsCatalogs,
                                                                                        x => x.CatalogId,
                                                                                        x => x.CatalogId,
                                                                                        (x, y) => new { x.OwnerId }).FirstOrDefault();

            if (catalog.OwnerId != ownerId)
            {
                return Result.Unauthorized();
            }

            return Result.Ok(question);
        }

        public Result<List<QuestionHeaderDTO>> GetQuestionsHeaders(long ownerId, long catalogId)
        {
            var catalog = context.QuestionsCatalogs.Where(x => x.CatalogId == catalogId).Select(x => new { x.OwnerId } ).FirstOrDefault();

            if (catalog == null)
            {
                return Result.NotFound();
            }
            if (catalog.OwnerId != ownerId)
            {
                return Result.Unauthorized();
            }

            var questions = context.Questions.Where(x => x.CatalogId == catalogId).Select(QuestionHeaderDTO.MappingExpr).ToList();

            return Result.Ok(questions);
        }

        public async Task<Result<List<QuestionHeaderDTO>>> GetQuestionsHeadersAsync(long ownerId, long catalogId)
        {
            var catalog = await context.QuestionsCatalogs.Where(x => x.CatalogId == catalogId).Select(x => new { x.OwnerId }).FirstOrDefaultAsync();

            if (catalog == null)
            {
                return Result.NotFound();
            }
            if (catalog.OwnerId != ownerId)
            {
                return Result.Unauthorized();
            }

            var questions = await context.Questions.Where(x => x.CatalogId == catalogId).Select(QuestionHeaderDTO.MappingExpr).ToListAsync();

            return Result.Ok(questions);
        }
    }
}

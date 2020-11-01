using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.Persistence;

namespace TestMe.TestCreation.App.RequestHandlers.Questions.ReadQuestions
{
    internal sealed class ReadQuestionsHandler : IRequestHandler<ReadQuestionsQuery, Result<OffsetPagedResults<QuestionOnListDTO>>>
    {
        private readonly ReadOnlyTestCreationDbContext context;

        public ReadQuestionsHandler(ReadOnlyTestCreationDbContext context)
        {
            this.context = context;
        }


        public async Task<Result<OffsetPagedResults<QuestionOnListDTO>>> Handle(ReadQuestionsQuery query, CancellationToken cancellationToken)
        {
            var catalog = await context.QuestionsCatalogs.Where(x => x.CatalogId == query.CatalogId).Select(x => new { x.OwnerId }).FirstOrDefaultAsync();

            if (catalog == null)
            {
                return Result.NotFound();
            }
            if (catalog.OwnerId != query.UserId)
            {
                return Result.Unauthorized();
            }

            var questions = await context.Questions.Where(x => x.CatalogId == query.CatalogId)
                                             .Skip(query.Pagination.Offset)
                                             .Take(query.Pagination.Limit + 1)                                             
                                             .Select(x => new QuestionOnListDTO() 
                                             { 
                                                 ConcurrencyToken = x.ConcurrencyToken,
                                                 Content = x.Content,
                                                 QuestionId = x.QuestionId 
                                             })
                                             .ToListAsync();

            return Result.Ok(new OffsetPagedResults<QuestionOnListDTO>(questions, query.Pagination.Limit));
        }
    }
}
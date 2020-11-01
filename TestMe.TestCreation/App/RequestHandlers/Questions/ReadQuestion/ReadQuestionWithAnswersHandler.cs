using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.Domain;
using TestMe.TestCreation.Persistence;

namespace TestMe.TestCreation.App.RequestHandlers.Questions.ReadQuestion
{
    internal sealed class ReadQuestionWithAnswersHandler : IRequestHandler<ReadQuestionWithAnswersQuery, Result<QuestionWithAnswersDTO>>
    {
        private readonly ReadOnlyTestCreationDbContext context;

        public ReadQuestionWithAnswersHandler(ReadOnlyTestCreationDbContext context)
        {
            this.context = context;
        }


        public async Task<Result<QuestionWithAnswersDTO>> Handle(ReadQuestionWithAnswersQuery query, CancellationToken cancellationToken)
        {
            Question question = await context.Questions.Where(x => x.QuestionId == query.QuestionId).FirstOrDefaultAsync();

            if (question == null)
            {
                return Result.NotFound();
            }

            var catalog = await context.Questions.Where(x => x.QuestionId == query.QuestionId).Join(context.QuestionsCatalogs,
                                                                                         x => x.CatalogId,
                                                                                         x => x.CatalogId,
                                                                                         (x, y) => new { x.OwnerId }).FirstOrDefaultAsync();

            if (catalog.OwnerId != query.UserId)
            {
                return Result.Unauthorized();
            }

            var dto = new QuestionWithAnswersDTO(question);

            return Result.Ok(dto);
        }
    }
}

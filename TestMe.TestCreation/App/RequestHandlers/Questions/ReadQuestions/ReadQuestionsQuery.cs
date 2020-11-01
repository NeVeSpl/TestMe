using MediatR;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.App.RequestEnrichers;

namespace TestMe.TestCreation.App.RequestHandlers.Questions.ReadQuestions
{   
    public sealed class ReadQuestionsQuery : IHaveUserId, IRequest<Result<OffsetPagedResults<QuestionOnListDTO>>>
    {
        public long UserId { get; set; }
        public long CatalogId { get; set; }
        public OffsetPagination Pagination { get; set; }

        public ReadQuestionsQuery(long catalogId, OffsetPagination pagination)
        {           
            CatalogId = catalogId;
            Pagination = pagination;
        }
    }   
}
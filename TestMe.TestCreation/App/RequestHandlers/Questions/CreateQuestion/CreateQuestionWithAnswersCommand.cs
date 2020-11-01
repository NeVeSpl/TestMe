using System.Collections.Generic;
using MediatR;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.App.RequestEnrichers;

namespace TestMe.TestCreation.App.RequestHandlers.Questions.CreateQuestion
{
    public sealed class CreateQuestionWithAnswersCommand : IHaveUserId, IRequest<Result<long>>
    {
        public long UserId { get; set; }
        public string Content { get; set; } = string.Empty;
        public List<CreateAnswer> Answers { get; set; } = new List<CreateAnswer>();
        public long CatalogId { get; set; }
    }
}
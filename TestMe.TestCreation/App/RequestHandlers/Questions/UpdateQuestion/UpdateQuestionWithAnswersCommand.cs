using System.Collections.Generic;
using MediatR;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.App.RequestEnrichers;

namespace TestMe.TestCreation.App.RequestHandlers.Questions.UpdateQuestion
{
    public sealed class UpdateQuestionWithAnswersCommand : IHaveUserId, IRequest<Result>
    {
        /// <summary>
        /// When ConcurrencyToken is not provided update will be forced (it will succeed even if concurrent edit happened)
        /// </summary>
        public uint? ConcurrencyToken { get; set; }

        public long QuestionId { get; set; }




        public long UserId { get; set; }
        public string Content { get; set; } = string.Empty;
        public List<UpdateAnswer> Answers { get; set; } = new List<UpdateAnswer>();
        public long CatalogId { get; set; }
    }
}

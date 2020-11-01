using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.App.RequestHandlers.Questions.ReadQuestions
{
    public sealed class QuestionOnListDTO : IHaveConcurrencyToken
    {
        public long QuestionId
        {
            get;
            set;
        }
        public string Content { get; set; } = string.Empty;
       
        public uint ConcurrencyToken { get; set; }


        internal QuestionOnListDTO()
        {

        }

        internal QuestionOnListDTO(Question question)
        {
            QuestionId = question.QuestionId;
            Content = question.Content;
            ConcurrencyToken = question.ConcurrencyToken;
        }
    }
}
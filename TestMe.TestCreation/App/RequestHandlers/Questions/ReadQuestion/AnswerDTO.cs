using System;
using System.Linq.Expressions;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.App.RequestHandlers.Questions.ReadQuestion
{
    public sealed class AnswerDTO
    {
        public long AnswerId
        {
            get;
            set;
        }
        public string Content { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }

        // old way, kept as a reminder
        //internal static readonly Expression<Func<Answer, AnswerDTO>> MappingExpr = x =>
        // new AnswerDTO
        // {
        //     AnswerId = x.AnswerId,
        //     Content = x.Content,
        //     IsCorrect = x.IsCorrect,
        // };
        //internal static readonly Func<Answer, AnswerDTO> Mapping = MappingExpr.Compile();

        internal AnswerDTO()
        {

        }
        internal AnswerDTO(Answer x)
        {
            AnswerId = x.AnswerId;
            Content = x.Content;
            IsCorrect = x.IsCorrect;
        }
    }
}

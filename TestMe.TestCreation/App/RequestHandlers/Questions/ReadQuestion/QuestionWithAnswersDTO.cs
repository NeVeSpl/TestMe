using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using TestMe.TestCreation.Domain;
using TestMe.BuildingBlocks.App;

namespace TestMe.TestCreation.App.RequestHandlers.Questions.ReadQuestion
{
    public sealed class QuestionWithAnswersDTO : IHaveConcurrencyToken
    {
        public long QuestionId
        {
            get;
            set;
        }
        public string Content { get; set; } = string.Empty;
        public List<AnswerDTO> Answers
        {
            get;
            set;
        } = new List<AnswerDTO>();

        public uint ConcurrencyToken { get; set; }



        internal QuestionWithAnswersDTO()
        {

        }
        internal QuestionWithAnswersDTO(Question question)
        {
            QuestionId = question.QuestionId;
            Content = question.Content;
            ConcurrencyToken = question.ConcurrencyToken;
            Answers = question.Answers.Select(x => new AnswerDTO(x)).ToList();
        }
    }
}
using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;

using TestMe.TestCreation.Domain;
using TestMe.BuildingBlocks.App;

namespace TestMe.TestCreation.App.Questions.Output
{
    public class QuestionDTO : QuestionHeaderDTO, IHaveConcurrencyToken
    {
        public List<AnswerDTO> Answers
        {
            get;
            set;
        } = new List<AnswerDTO>();

        public uint ConcurrencyToken { get; set; }



        internal new static readonly Expression<Func<Question, QuestionDTO>> MappingExpr = x =>
          new QuestionDTO
          {
              QuestionId = x.QuestionId,
              Content = x.Content,
              ConcurrencyToken = x.ConcurrencyToken
          };

        internal static readonly Func<Question, QuestionDTO> Mapping = x =>
          new QuestionDTO
          {
              QuestionId = x.QuestionId,
              Content = x.Content,
              ConcurrencyToken = x.ConcurrencyToken,
              Answers = x.Answers.Select(AnswerDTO.Mapping).ToList()
          };


    }
}

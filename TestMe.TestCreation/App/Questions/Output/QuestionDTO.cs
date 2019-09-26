using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using TestMe.SharedKernel.App;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.App.Questions.Output
{
    public class QuestionDTO : QuestionHeaderDTO, IHasConcurrencyToken
    {       
        public List<AnswerDTO> Answers
        {
            get;
            set;
        }

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

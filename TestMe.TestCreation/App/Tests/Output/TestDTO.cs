using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TestMe.TestCreation.Domain;
using System.Linq;

namespace TestMe.TestCreation.App.Tests.Output
{
    public class TestDTO : TestHeaderDTO
    {
        public List<QuestionItemDTO> QuestionItems { get; set; }








        internal new static readonly Expression<Func<Test, TestDTO>> Mapping = x =>
           new TestDTO
           {
               TestId = x.TestId,
               Title = x.Title,           
           };
    }
}

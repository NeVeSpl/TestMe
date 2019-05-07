using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.App.Tests.Output
{
    public class TestHeaderDTO
    {
        public long TestId { get; set; }
        public string Title { get; set; }






        internal static readonly Expression<Func<Test, TestHeaderDTO>> Mapping = x =>
           new TestHeaderDTO
           {
               TestId = x.TestId,
               Title = x.Title
           };
    }
}

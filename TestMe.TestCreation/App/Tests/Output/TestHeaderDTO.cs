using System;
using System.Linq.Expressions;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.App.Tests.Output
{
    public class TestHeaderDTO
    {
        public long TestId { get; set; }
        public string Title { get; set; } = string.Empty;






        internal static readonly Expression<Func<Test, TestHeaderDTO>> MappingExpr = x =>
           new TestHeaderDTO
           {
               TestId = x.TestId,
               Title = x.Title
           };
    }
}

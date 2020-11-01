using System;
using System.Linq.Expressions;

namespace TestMe.TestCreation.App.RequestHandlers.Tests.ReadTest
{
    public class TestDTO
    {
        public long TestId { get; set; }
        public string Title { get; set; } = string.Empty;
        








        internal static readonly Expression<Func<Domain.Test, TestDTO>> MappingExpr = x =>
           new TestDTO
           {
               TestId = x.TestId,
               Title = x.Title,           
           };
        internal static readonly Func<Domain.Test, TestDTO> MapFrom = MappingExpr.Compile();
    }
}
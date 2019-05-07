using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TestMe.SharedKernel.App;
using TestMe.TestCreation.App.Tests.Output;
using TestMe.TestCreation.Persistence;

namespace TestMe.TestCreation.App.Tests
{
    internal sealed class TestReader 
    {
        private readonly TestCreationDbContext context;


        public TestReader(TestCreationDbContext context)
        {
            this.context = context;            
        }


        public Result<List<TestHeaderDTO>> GetTestHeaders(long ownerId, long catalogId)
        {
            var catalog = context.TestsCatalogs.Where(x => x.CatalogId == catalogId).Select(x => new { x.OwnerId }).FirstOrDefault();

            if (catalog == null)
            {
                return Result.NotFound();
            }
            if (catalog.OwnerId != ownerId)
            {
                return Result.Unauthorized();
            }

            var tests = context.Tests.AsNoTracking().Where(x => x.CatalogId == catalogId).Select(TestHeaderDTO.Mapping).ToList();

            return Result.Ok(tests);
        }

        public Result<TestDTO> GetTest(long ownerId, long testId, bool includeQuestionItemsWithQuestionHeaders)
        {
            TestDTO test = context.Tests.AsNoTracking().Where(x => x.TestId == testId).Select(TestDTO.Mapping).FirstOrDefault();

            if (test == null)
            {
                return Result.NotFound();
            }

            var catalog = context.Tests.Where(x => x.TestId == testId).Join(context.TestsCatalogs,
                                                                            x => x.CatalogId,
                                                                            x => x.CatalogId,
                                                                            (x, y) => new { x.OwnerId }).FirstOrDefault();

            if (catalog.OwnerId != ownerId)
            {
                return Result.Unauthorized();
            }

            if (includeQuestionItemsWithQuestionHeaders)
            {
                test.QuestionItems = context.QuestionItems.AsNoTracking()
                                                          .Include(x => x.Question)
                                                          .IgnoreQueryFilters()
                                                          .Where(x => x.TestId == testId)
                                                          .Select(QuestionItemDTO.Mapping)
                                                          .ToList();
            }                             

            return Result.Ok(test);
        }

    }
}

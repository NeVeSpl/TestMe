﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TestMe.SharedKernel.App;
using TestMe.TestCreation.App.Tests.Output;
using TestMe.TestCreation.Domain;
using TestMe.TestCreation.Persistence;

namespace TestMe.TestCreation.App.Tests
{
    internal sealed class TestReader 
    {
        private readonly ReadOnlyTestCreationDbContext context;


        public TestReader(ReadOnlyTestCreationDbContext context)
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

            var tests = context.Tests.Where(x => x.CatalogId == catalogId).Select(TestHeaderDTO.MappingExpr).ToList();

            return Result.Ok(tests);
        }

        public Result<TestDTO> GetTest(long ownerId, long testId, bool includeQuestionItemsWithQuestionHeaders)
        {
            Test test = context.Tests.Where(x => x.TestId == testId).FirstOrDefault();            

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

            TestDTO dto = TestDTO.Mapping(test);

            if (includeQuestionItemsWithQuestionHeaders)
            {
                // To do
                //test.QuestionItems = context.QuestionItems.Include(x => x.Question)
                //                                          .IgnoreQueryFilters()
                //                                          .Where(x => x.TestId == testId)
                //                                          .Select(QuestionItemDTO.Mapping)
                //                                          .ToList();
            }                             

            return Result.Ok(dto);
        }

    }
}

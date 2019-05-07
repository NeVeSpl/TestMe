﻿using System.Linq;
using Microsoft.EntityFrameworkCore;
using TestMe.SharedKernel.Persistence;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.Persistence.Repositories
{
    internal sealed class TestRepository : GenericRepository<Test, TestCreationDbContext>, ITestRepository
    {
        public TestRepository(TestCreationDbContext context) : base(context)
        {
          
        }


        public Test GetByIdWithTestItems(long id)
        {
            return context.Tests.Include(x => x.Questions).FirstOrDefault(x => x.TestId == id);
        }
    }
}

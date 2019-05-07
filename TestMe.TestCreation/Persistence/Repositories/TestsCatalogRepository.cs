using System.Linq;
using Microsoft.EntityFrameworkCore;
using TestMe.SharedKernel.Persistence;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.Persistence.Repositories
{
    internal sealed class TestsCatalogRepository : GenericRepository<TestsCatalog, TestCreationDbContext>, ITestsCatalogRepository
    {
        public TestsCatalogRepository(TestCreationDbContext context) : base(context)
        {

        }


        public TestsCatalog GetById(long id, bool includeTests = false)
        {
            if (includeTests)
            {
                return context.TestsCatalogs.Include(x => x.Tests).FirstOrDefault(x => x.CatalogId == id);
            }
            return base.GetById(id);
        }
    }
}

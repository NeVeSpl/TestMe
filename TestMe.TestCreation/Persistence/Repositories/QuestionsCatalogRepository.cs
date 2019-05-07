using System.Linq;
using Microsoft.EntityFrameworkCore;
using TestMe.SharedKernel.Persistence;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.Persistence.Repositories
{
    internal sealed class QuestionsCatalogRepository : GenericRepository<QuestionsCatalog, TestCreationDbContext>, IQuestionsCatalogRepository
    {
        public QuestionsCatalogRepository(TestCreationDbContext context) : base(context)
        {
           
        }

        public QuestionsCatalog GetById(long id, bool includeQuestions = false)
        {
            if (includeQuestions)
            {
                return context.QuestionsCatalogs
                               .Include(x => x.Questions)                           
                               .FirstOrDefault(x => x.CatalogId == id);
            }
            return base.GetById(id);
        }
    }
}

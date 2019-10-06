using System.Linq;
using Microsoft.EntityFrameworkCore;
using TestMe.SharedKernel.Persistence;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.Persistence
{
    internal sealed class QuestionRepository : GenericRepository<Question, TestCreationDbContext>, IQuestionRepository
    {
        public QuestionRepository(TestCreationDbContext context) : base(context)
        {
           
        }
       

        public Question GetByIdWithAnswers(long id)
        {
            return context.Questions.Include(x => x.Answers).FirstOrDefault(x => x.QuestionId == id);
        }
    }
}

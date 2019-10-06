using TestMe.SharedKernel.Persistence;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.Persistence
{
    internal sealed class OwnerRepository : GenericRepository<Owner, TestCreationDbContext>, IOwnerRepository
    {
        public OwnerRepository(TestCreationDbContext context) : base(context)
        {

        }
    }
}

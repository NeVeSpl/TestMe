using System.Linq;
using Microsoft.EntityFrameworkCore;
using TestMe.BuildingBlocks.Persistence;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.Persistence
{
    internal sealed class OwnerRepository : GenericRepository<Owner, TestCreationDbContext>, IOwnerRepository
    {
        public OwnerRepository(TestCreationDbContext context) : base(context)
        {

        }

        public Owner GetByIdWithTestsCatalogs(long ownerId)
        {
            return context.Owners.Where(x => x.OwnerId == ownerId).Include(x => x.TestsCatalogs).FirstOrDefault();
        }
    }
}

using Microsoft.EntityFrameworkCore;

namespace TestMe.SharedKernel.Persistence
{
    public abstract class GenericRepository<TEntity, TContext> where TEntity : class
                                                               where TContext : DbContext
    {
        protected readonly TContext context;


        public GenericRepository(TContext context)
        {
            this.context = context;
        }


        public void Add(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
        }
        public TEntity GetById(long id)
        {
            return context.Set<TEntity>().Find(id);
        }
    }
}

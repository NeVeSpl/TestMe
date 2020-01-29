namespace Microsoft.EntityFrameworkCore
{
    public static class DbContextExtensions
    {
        public static void DisableTracking(this DbContext context)
        {
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
    }
}

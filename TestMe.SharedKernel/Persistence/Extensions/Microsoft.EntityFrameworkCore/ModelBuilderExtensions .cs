using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore
{
    public static class ModelBuilderExtensions
    {
        public static void RemovePluralizingTableNameConvention(this ModelBuilder modelBuilder)
        {
            foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
            {               
                // EF core 3.0 supports only one type of inheritence: table per hierarchy (TPH)
                // thus we set table name only for root entity
                if (entity.ClrType.BaseType.Name == "Object")
                {
                    string displayName = entity.DisplayName();                    
                    entity.SetTableName(displayName);
                }
            }
        }
    }
}

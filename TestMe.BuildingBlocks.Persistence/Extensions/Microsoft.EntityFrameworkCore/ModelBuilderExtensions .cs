using System;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore
{
    public static class ModelBuilderExtensions
    {
        [Obsolete("It does not work well with EF core 3.0 owned types")]
        public static void RemovePluralizingTableNameConvention(this ModelBuilder modelBuilder)
        {
            foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
            {               
                // EF core 3.0 supports only one type of inheritance: table per hierarchy (TPH)
                // thus we set table name only for root entity
                if (entity.ClrType?.BaseType?.Name == "Object")
                {
                    string tableName = entity.GetTableName();
                    string displayName = entity.DisplayName();
                    if (tableName != displayName)
                    {
                        entity.SetTableName(displayName);
                    }
                }
            }            
        }
    }
}

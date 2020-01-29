using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestMe.BuildingBlocks.EventBus;

namespace TestMe.TestCreation.Persistence.Inbox
{
    internal sealed class CatalogConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.Property(x => x.EventId).ValueGeneratedNever();            
        }
    }
}

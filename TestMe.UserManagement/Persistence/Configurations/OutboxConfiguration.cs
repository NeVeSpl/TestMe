using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using TestMe.BuildingBlocks.EventBus;

namespace TestMe.UserManagement.Persistence.Configurations
{
    internal sealed class OutboxConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.Property(x => x.EventId).HasValueGenerator<SequentialGuidValueGenerator>(); 
            builder.HasIndex(x => new { x.PostDateTime, x.CorrelationId }).HasFilter($"\"{nameof(Event.PostDateTime)}\" IS NULL");
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestMe.UserManagement.Domain;

namespace TestMe.Core.Persistence.UserManagement.Configurations
{
    internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(User.NameMaxLength);
            builder.HasIndex(nameof(User.Name));
        }
    }
}

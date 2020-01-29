using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestMe.UserManagement.Domain;

namespace TestMe.UserManagement.Persistence.Configurations
{
    internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {            
            builder.Property(x => x.Name).HasMaxLength(User.NameMaxLength);           
            builder.OwnsOne(x => x.EmailAddress, emailBuilder => 
            { 
                emailBuilder.WithOwner();
                emailBuilder.HasIndex(x => x.Value).IsUnique();
                emailBuilder.Property(x => x.Value).HasMaxLength(EmailAddress.MaxLength);
            });
            builder.OwnsOne(x => x.Password).WithOwner();
        }
    }
}
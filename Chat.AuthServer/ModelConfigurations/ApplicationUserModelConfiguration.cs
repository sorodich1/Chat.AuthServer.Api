using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.AuthServer.ModelConfigurations
{
    public class ApplicationUserModelConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(x => x.LastName).HasMaxLength(128).IsRequired();
            builder.Property(x => x.FirstName).HasMaxLength(128).IsRequired();
            builder.Property(x => x.ApplicationUserId).IsRequired(false);

            builder.HasOne(x => x.ApplicationUserProfile);
        }
    }
}

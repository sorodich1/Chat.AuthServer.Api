using Chat.AuthServer.ModelConfigurations.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Chat.AuthServer.ModelConfigurations
{
    public class ApplicationUserProfileModelConfiguration : AuditableModelConfigurationBase<ApplicationUserProfile>
    {
        protected override void AddBuilder(EntityTypeBuilder<ApplicationUserProfile> builder)
        {
            builder.HasMany(x => x.Permission);

            builder.HasOne(x => x.ApplicationUser)
                .WithOne(x => x.ApplicationUserProfile)
                .HasForeignKey<ApplicationUser>(x => x.ApplicationUserId);
        }
        protected override string TableName()
        {
            return "ApplicationUserProfile";
        }
    }
}

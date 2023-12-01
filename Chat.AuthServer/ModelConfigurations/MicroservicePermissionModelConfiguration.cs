using Chat.AuthServer.ModelConfigurations.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.AuthServer.ModelConfigurations
{
    public class MicroservicePermissionModelConfiguration : AuditableModelConfigurationBase<MicroservicePermission>
    {
        protected override void AddBuilder(EntityTypeBuilder<MicroservicePermission> builder)
        {
            builder.Property(x => x.PolicyName).HasMaxLength(64).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(1024);
            builder.Property(x => x.ApplicationUserProfileId).IsRequired();

            builder.HasOne(x => x.ApplicationUserProfile).WithMany(x => x.Permission);
        }

        protected override string TableName()
        {
            return "MicroservicePermissions";
        }
    }
}

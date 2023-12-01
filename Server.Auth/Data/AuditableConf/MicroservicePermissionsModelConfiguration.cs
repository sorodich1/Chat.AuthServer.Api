using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Auth.Data.Entityes;

namespace Server.Auth.Data.AuditableConf
{
    //public class MicroservicePermissionsModelConfiguration : AuditableConfigurationBase<MicroservicePermission>
    //{
    //    protected override void AddBuilder(EntityTypeBuilder<MicroservicePermission> builder)
    //    {
    //        //builder.Property(x => x.PolicyName).HasMaxLength(64).IsRequired();
    //        //builder.Property(x => x.Description).HasMaxLength(1064);
    //        //builder.Property(x => x.ApplicationUserProfile).IsRequired();

    //        //builder.HasOne(x => x.ApplicationUserProfile).WithMany(x => x.Permissions);
    //    }

    //    protected override string TableName()
    //    {
    //        return "MicroservicePermission";
    //    }
    //}
}

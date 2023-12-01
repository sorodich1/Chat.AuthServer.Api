using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Server.Auth.Data.AuditableConf
{
    /// <summary>
    /// База конфигурации модели с возможностью аудита
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AuditableConfigurationBase<T> : IEntityTypeConfiguration<T> where T : AuditableConf.Auditable
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            //builder.ToTable(TableName());
            //builder.HasKey(x => x.Id);
            //builder.Property(x => x.Id).IsRequired();

            //builder.Property(x => x.CreateAt).IsRequired().HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc)).IsRequired();
            //builder.Property(x => x.CreateBy).HasMaxLength(256).IsRequired();
            //builder.Property(x => x.UpdateAt).HasConversion(v => v.Date, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
            //builder.Property(x => x.UpdateBy).HasMaxLength(256);

            //AddBuilder(builder);
        }

        /// <summary>
        /// Добавьте настраиваемые свойства для вашей сущности
        /// </summary>
        /// <param name="builder"></param>
        protected abstract void AddBuilder(EntityTypeBuilder<T> builder);

        protected abstract string TableName();
    }
}

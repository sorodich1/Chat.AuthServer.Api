using Calabonga.EntityFrameworkCore.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.AuthServer.ModelConfiguration.Base
{
    /// <summary>
    /// База конфигурации модели с возможностью аудита
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class IdentityModelConfigurationBase<T> : IEntityTypeConfiguration<T> where T : Identity
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.ToTable(TableName());
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired();

            AddBuilder(builder);
        }

        /// <summary>
        /// Добавьте настраиваемые свойства для вашей сущности
        /// </summary>
        /// <param name="builder"></param>
        protected abstract void AddBuilder(EntityTypeBuilder<T> builder);

        /// <summary>
        /// Имя таблицы
        /// </summary>
        protected abstract string TableName();
    }
}

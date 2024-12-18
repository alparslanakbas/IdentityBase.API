namespace Identity.Api.Context.EntityConfigurations
{
    public class RoleEntityConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(x => x.Id);

            // Name configuration
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50)
                .HasComment("Rol adı")
                .HasAnnotation("ErrorMessage", "Rol adı zorunludur ve maksimum 50 karakter olmalıdır.");

            // Description configuration
            builder.Property(x => x.Description)
                .HasMaxLength(500)
                .HasComment("Rol açıklaması")
                .HasAnnotation("ErrorMessage", "Rol açıklaması 500 karakteri geçemez.");

            // Permission configuration
            builder.Property(x => x.Permissions)
                .HasMaxLength(1000)
                .HasColumnType("jsonb")
                .HasComment("Rol izinleri")
                .HasAnnotation("ErrorMessage", "Rol izinleri 500 karakteri geçemez.");

            // IsActive configuration
            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .HasComment("Rol aktiflik durumu");

            // IsDefault configuration
            builder.Property(x => x.IsDefault)
                .IsRequired()
                .HasDefaultValue(false)
                .HasComment("Varsayılan rol durumu");

            // CreatedAt configuration
            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasDefaultValue(DateTime.UtcNow)
                .HasComment("Oluşturulma tarihi");

            // UpdatedAt configuration
            builder.Property(x => x.UpdatedAt)
                .HasComment("Güncellenme tarihi");

            // DisplayOrder configuration
            builder.Property(x => x.DisplayOrder)
                .IsRequired()
                .HasDefaultValue(0)
                .HasComment("Görüntülenme sırası");

            // Level configuration
            builder.Property(x => x.Level)
                .IsRequired()
                .HasDefaultValue(0)
                .HasComment("Rol seviyesi");

            // Indexes
            builder.HasIndex(x => x.Name)
                .IsUnique()
                .HasDatabaseName("UK_Roles_Name");

            builder.HasIndex(x => x.IsActive);
            builder.HasIndex(x => x.Level);

            // Global query filter
            builder.HasQueryFilter(x => !x.IsDeleted);

            builder.ToTable("Roles", x => x.HasComment("Rol bilgilerinin tutulduğu tablo"));
        }
    }
}

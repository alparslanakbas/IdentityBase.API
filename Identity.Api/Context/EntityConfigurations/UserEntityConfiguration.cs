namespace Identity.Api.Context.EntityConfigurations
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            // Username configuration
            builder.Property(x => x.UserName)
                .IsRequired()
                .HasMaxLength(70)
                .HasComment("Kullanıcı Adı")
                .HasAnnotation("ErrorMessage", "Kullanıcı adı zorunludur ve maksimum 70 karakter olmalıdır.");
                

            // Email configuration
            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(100)
                .HasComment("E-posta adresi")
                .HasAnnotation("ErrorMessage", "Geçerli bir e-posta adresi girilmelidir.");

            // Join Date configuration
            builder.Property(x => x.JoinDate)
                .IsRequired()
                .HasDefaultValue(DateTime.UtcNow)
                .HasComment("Kayıt tarihi")
                .HasAnnotation("ErrorMessage", "Kayıt tarihi geçerli bir tarih olmalıdır.");

            // ProfilePicture configuration
            builder.Property(x => x.ProfilPicture)
                .HasMaxLength(500)
                .HasComment("Profil fotoğrafı URL'i")
                .HasAnnotation("ErrorMessage", "Profil fotoğrafı URL'i 500 karakteri geçemez.");

            // LastLoginDate configuration
            builder.Property(x => x.LastLoginDate)
                .HasComment("Son giriş tarihi");

            // RefreshToken configuration
            builder.Property(x => x.RefreshToken)
                .HasMaxLength(128)
                .HasComment("JWT Refresh Token");

            builder.Property(x => x.RefreshTokenExpiryTime)
                .HasComment("Refresh Token son geçerlilik tarihi");

            // Active status configuration
            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(false)
                .HasComment("Kullanıcı aktiflik durumu");

            // Soft delete configuration
            builder.Property(x => x.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false)
                .HasComment("Silinme durumu");

            builder.Property(x => x.DeletedAt)
                .HasComment("Silinme tarihi");

            // Update tracking
            builder.Property(x => x.UpdatedAt)
                .HasComment("Son güncelleme tarihi");

            // Indexes
            builder.HasIndex(x => x.UserName)
                .IsUnique()
                .HasDatabaseName("UK_Users_UserName");

            builder.HasIndex(x => x.Email)
                .IsUnique()
                .HasDatabaseName("UK_Users_Email");

            builder.HasIndex(x => x.IsActive)
                .HasDatabaseName("IX_Users_IsActive");

            builder.HasIndex(x => x.IsDeleted)
                .HasDatabaseName("IX_Users_IsDeleted");

            // Global query filter
            builder.HasQueryFilter(x => !x.IsDeleted);

            // Table configuration
            builder.ToTable("Users", b => b.HasComment("Kullanıcı bilgilerinin tutulduğu tablo"));
        }
    }
}

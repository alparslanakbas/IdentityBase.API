namespace Identity.Api.Models
{
    public class User : IdentityUser<Guid>
    {
        public bool IsActive { get; set; }
        public string? ProfilPicture { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime LastLoginDate { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}

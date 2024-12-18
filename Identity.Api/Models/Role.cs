namespace Identity.Api.Models
{
    public class Role : IdentityRole<Guid>
    {
        public string? Description { get; set; }
        public bool IsActive { get; set; } 
        public bool IsDefault { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public bool IsDeleted { get; set; } 
        public DateTime? DeletedAt { get; set; }
        public Guid? DeletedBy { get; set; }
        public int DisplayOrder { get; set; }
        public int Level { get; set; }
        public string? Permissions { get; set; }
    }
}

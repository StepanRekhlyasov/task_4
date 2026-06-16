using Microsoft.AspNetCore.Identity;

namespace backend.Models.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; } = "";
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime LastActivityAt { get; set; }
    }
}

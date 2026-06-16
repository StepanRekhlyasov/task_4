using backend.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public class AuthDbContext(DbContextOptions<AuthDbContext> options) : IdentityDbContext<User>(options)
    {
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<User>().Where(e => e.State == EntityState.Added))
            {
                var user = entry.Entity;
                
                user.CreatedAt = now;
                user.UpdatedAt = now;
                user.LastActivityAt = now;

            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}

using AuthProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthProject.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        public DbSet<Recipe> Recipes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var chefid = "0f403930-3748-44d6-9571-6515cef0aed6";
            var userid = "ae383bd0-d66e-497b-b675-13c172b421b2";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = chefid,
                    Name = "Chef",
                    NormalizedName = "CHEF".ToUpper(),
                    ConcurrencyStamp = chefid
                },
                new IdentityRole
                {
                    Id = userid,
                    Name = "User",
                    NormalizedName = "USER".ToUpper(),
                    ConcurrencyStamp = userid
                }
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}

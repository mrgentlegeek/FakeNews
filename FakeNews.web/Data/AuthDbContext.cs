using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FakeNews.web.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Seed Roles (User, Admin, SuperAdmin)

            var adminRoleId = "23ea0903-22c9-4725-af61-faa002d973b5";
            var superAdminRoleId = "d54ab7bb-2158-42f9-833a-713b4946331b";
            var userRoleId = "4be2aa2c-d12e-41d8-acc5-705b51b18688";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName="Admin",
                    Id= adminRoleId,
                    ConcurrencyStamp = adminRoleId,
                },
                new IdentityRole
                {
                    Name = "SuperAdmin",
                    NormalizedName="SuperAdmin",
                    Id= superAdminRoleId,
                    ConcurrencyStamp = superAdminRoleId,
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName="User",
                    Id= userRoleId,
                    ConcurrencyStamp = userRoleId,
                },
            };

            //when migration is ran, these roles will be inserted into DB
            builder.Entity<IdentityRole>().HasData(roles);


            //Seed SuperAdminUser
            var superAdminId = "e8910d8a-fd2b-4a9d-8023-5e0498ca5dfa";

            var superAdminUser = new IdentityUser
            {
                UserName = "superadmin@fakenews.com",
                Email = "superadmin@fakenews.com",
                NormalizedUserName = "superadmin@fakenews.com".ToUpper(),
                NormalizedEmail = "superadmin@fakenews.com".ToUpper(),
                Id = superAdminId,
            };

            //Add password to superadmin
            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(superAdminUser, "SuperAmin123");

            builder.Entity<IdentityUser>().HasData(superAdminUser);


            // Add all roles to the SuperAdminUser
            var superAdminRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId = userRoleId,
                    UserId = superAdminId,
                },
                new IdentityUserRole<string>
                {
                    RoleId = adminRoleId,
                    UserId = superAdminId,
                },
                new IdentityUserRole<string>
                {
                    RoleId = superAdminRoleId ,
                    UserId = superAdminId,
                },
            };

            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);
        }
    }
}

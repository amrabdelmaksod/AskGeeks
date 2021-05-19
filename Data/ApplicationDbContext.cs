using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UserManagementWithIdentity.Models;

namespace UserManagementWithIdentity.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
           
            
            builder.Entity<ApplicationUser>()
                .ToTable("Users", "security");

            builder.Entity<IdentityRole>()
                .ToTable("Roles", "security");

            builder.Entity<IdentityUserRole<string>>()
                .ToTable("UserRoles", "security");

            builder.Entity<IdentityUserClaim<string>>()
                .ToTable("UserClaims", "security");

            builder.Entity<IdentityUserLogin<string>>()
               .ToTable("UserLogins", "security");

            builder.Entity<IdentityRoleClaim<string>>()
               .ToTable("RoleClaims", "security");

            builder.Entity<IdentityUserToken<string>>()
               .ToTable("UserTokens", "security");

           //builder.Entity<ApplicationUser>()
           //     .HasData(new ApplicationUser { Id= "d6d25e3f-b9ff-45b2-9cd9-d3387a157337" , UserName= "Admin", NormalizedUserName="ADMIN", Email= "Admin@admin.com", NormalizedEmail= "Admin@admin.com".ToUpper(), EmailConfirmed = false , PasswordHash = "AQAAAAEAACcQAAAAEJ0OsXuI/m9sjMPo/BP6mWW5/NsEdyoZMwlGSmXcFl4Ncyp8KCLkGA/VNZa6MrP1yg==", SecurityStamp= "QBVNRKT43ZBTOSWEKAKMFN2GHJSN2CWQ",ConcurrencyStamp= "0869f100-11a2-47d2-adc4-15b1877e8907", PhoneNumber=null, PhoneNumberConfirmed= false, TwoFactorEnabled=false, LockoutEnabled = null })
        }
    }
}

using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SantasWishList.Data.Models;

namespace SantasWishList.Data
{
	public class DatabaseContext : IdentityDbContext<User, Role, int>
	{
		private IConfiguration Configuration => new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json")
			.Build();
		
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
				optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DatabaseContext"));
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

            int uid = 1;

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = uid,
                    UserName = "santa",
                    NormalizedUserName = "SANTACLAUSE",
                    PasswordHash = new PasswordHasher<User>().HashPassword(null, "wachtwoord")
                }
            );

            modelBuilder.Entity<Role>().HasData(
                new
                {
                    Id = 1,
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new
                {
                    Id = 2,
                    Name = "Temp",
                    NormalizedName = "TEMP"
                }
            );

            modelBuilder.Entity<IdentityUserRole<int>>().HasData(
                new IdentityUserRole<int>
                {
                    UserId = uid,
                    RoleId = 1
                });
        }
	}
}


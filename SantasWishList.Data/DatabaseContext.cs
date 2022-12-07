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
			modelBuilder.Entity<User>(user => user.ToTable("User"));
			// Seed data
			Seeder seeder = new Seeder();
			modelBuilder.Entity<User>().HasData(seeder.UserSeeder());
			modelBuilder.Entity<Role>().HasData(seeder.RoleSeeder());
			modelBuilder.Entity<IdentityUserRole<int>>().HasData(seeder.IdentityUserRoleSeeder());
		}
	}
}


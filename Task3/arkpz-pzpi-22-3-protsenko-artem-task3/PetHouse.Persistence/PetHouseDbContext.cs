using Microsoft.EntityFrameworkCore;
using PetHouse.Core.Models;
using PetHouse.Persistence.Configurations;

namespace PetHouse.Persistence
{
   public class PetHouseDbContext : DbContext
   {
      public PetHouseDbContext(DbContextOptions<PetHouseDbContext> options) : base(options)
      {
      }

      public DbSet<User> Users { get; set; }
      public DbSet<Pet> Pets { get; set; }
      public DbSet<Meal> Meals { get; set; }
      public DbSet<Device> Devices { get; set; }
      public DbSet<HealthAnalysis> HealthAnalyses { get; set; }

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
         modelBuilder.ApplyConfiguration(new UserConfiguration());
         modelBuilder.ApplyConfiguration(new MealConfiguration());
         modelBuilder.ApplyConfiguration(new PetConfiguration());
         modelBuilder.ApplyConfiguration(new HealthAnalysisConfiguration());
         modelBuilder.ApplyConfiguration(new DeviceConfiguration());
      }
   }
}
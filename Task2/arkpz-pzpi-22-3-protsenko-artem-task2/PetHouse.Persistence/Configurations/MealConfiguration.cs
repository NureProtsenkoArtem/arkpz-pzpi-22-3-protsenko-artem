using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHouse.Core.Models;

namespace PetHouse.Persistence.Configurations;

public class MealConfiguration : IEntityTypeConfiguration<Meal>
{
   public void Configure(EntityTypeBuilder<Meal> builder)
   {
      builder.HasKey(m => m.MealId);
   }
}
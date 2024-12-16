using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHouse.Core.Models;

namespace PetHouse.Persistence.Configurations;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
   public void Configure(EntityTypeBuilder<Pet> builder)
   {
      builder.HasKey(p => p.PetId);

      builder.HasMany(p => p.Meals)
         .WithOne(m => m.Pet)
         .HasForeignKey(m => m.PetId)
         .OnDelete(DeleteBehavior.Cascade);

      builder.HasMany(p => p.PetHealthAnalyses)
         .WithOne(h => h.Pet)
         .HasForeignKey(h => h.PetId)
         .OnDelete(DeleteBehavior.Cascade);
   }
}
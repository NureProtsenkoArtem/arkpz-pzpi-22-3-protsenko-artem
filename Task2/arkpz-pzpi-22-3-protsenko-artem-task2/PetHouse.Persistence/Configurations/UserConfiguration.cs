using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHouse.Core.Models;

namespace PetHouse.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
   public void Configure(EntityTypeBuilder<User> builder)
   {
      builder.HasKey(u => u.UserId);
      
      builder.HasMany(u => u.UserPets) 
         .WithOne(p => p.User)
         .HasForeignKey(p => p.UserId)
         .OnDelete(DeleteBehavior.Cascade);

      builder.HasMany(u => u.UserDevices)
         .WithOne(d => d.User)
         .HasForeignKey(d => d.UserId)
         .OnDelete(DeleteBehavior.Cascade);
   }
}
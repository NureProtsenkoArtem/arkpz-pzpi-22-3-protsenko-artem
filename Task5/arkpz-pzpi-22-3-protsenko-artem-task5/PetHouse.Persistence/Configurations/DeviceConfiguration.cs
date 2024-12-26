using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHouse.Core.Models;

namespace PetHouse.Persistence.Configurations;

public class DeviceConfiguration : IEntityTypeConfiguration<Device>
{
   public void Configure(EntityTypeBuilder<Device> builder)
   {
      builder.HasKey(d => d.DeviceId);
      
   }
}
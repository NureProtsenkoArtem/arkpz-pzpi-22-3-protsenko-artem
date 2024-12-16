using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHouse.Core.Models;

namespace PetHouse.Persistence.Configurations;

public class HealthAnalysisConfiguration : IEntityTypeConfiguration<HealthAnalysis>
{
   public void Configure(EntityTypeBuilder<HealthAnalysis> builder)
   {
      builder.HasKey(h => h.HealthAnalysisId);
   }
}
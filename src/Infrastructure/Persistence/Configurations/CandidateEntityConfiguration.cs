using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Saiketsu.Service.Candidate.Domain.Entities;

namespace Saiketsu.Service.Candidate.Infrastructure.Persistence.Configurations;

internal class CandidateEntityConfiguration : IEntityTypeConfiguration<CandidateEntity>
{
    public void Configure(EntityTypeBuilder<CandidateEntity> builder)
    {
        builder.ToTable("candidate");

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(50);
    }
}
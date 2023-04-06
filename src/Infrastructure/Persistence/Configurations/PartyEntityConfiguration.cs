using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Saiketsu.Service.Candidate.Domain.Entities;

namespace Saiketsu.Service.Candidate.Infrastructure.Persistence.Configurations;

public sealed class PartyEntityConfiguration : IEntityTypeConfiguration<PartyEntity>
{
    public void Configure(EntityTypeBuilder<PartyEntity> builder)
    {
        builder.ToTable("party");

        builder.Property(x => x.Id)
            .IsRequired();

        builder.HasMany(x => x.Candidates)
            .WithOne(x => x.Party);
    }
}
using Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations.Read;

internal sealed class FollowerReadModelConfiguration : IEntityTypeConfiguration<FollowerReadModel>
{
    public void Configure(EntityTypeBuilder<FollowerReadModel> builder)
    {
        builder.HasKey(f => new { f.UserId, f.FollowedId });

        builder.HasOne(f => f.User)
            .WithMany()
            .HasForeignKey(f => f.UserId);

        builder.HasOne(f => f.Followed)
            .WithMany()
            .HasForeignKey(f => f.FollowedId);
    }
}

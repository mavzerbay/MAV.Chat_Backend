using MAV.Chat.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MAV.Chat.Infrastructure.Config
{
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.HasMany(x => x.Connections)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

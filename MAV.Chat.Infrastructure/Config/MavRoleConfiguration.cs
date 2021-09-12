using MAV.Chat.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MAV.Chat.Infrastructure.Config
{
    public class MavRoleConfiguration : IEntityTypeConfiguration<MavRole>
    {
        public void Configure(EntityTypeBuilder<MavRole> builder)
        {
            builder.HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
        }
    }
}

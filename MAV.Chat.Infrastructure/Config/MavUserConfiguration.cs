using MAV.Chat.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAV.Chat.Infrastructure.Config
{
   public class MavUserConfiguration : IEntityTypeConfiguration<MavUser>
    {
        public void Configure(EntityTypeBuilder<MavUser> builder)
        {
            builder.HasQueryFilter(x => x.DeletedById == null);

            builder.HasMany(ur => ur.UserRoles)
                   .WithOne(u => u.User)
                   .HasForeignKey(ur => ur.UserId)
                   .IsRequired();
        }
    }
}

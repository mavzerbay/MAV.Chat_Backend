using MAV.Chat.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MAV.Chat.Infrastructure.Config
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasOne(u => u.Receiver)
                .WithMany(m => m.ReceivedMessages)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(u => u.Sender)
                .WithMany(m => m.SendedMessages)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

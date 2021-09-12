using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MAV.Chat.Core.Entities
{
    public class Message : BaseEntity
    {
        public int SenderId { get; set; }
        [ForeignKey("SenderId")]
        public MavUser Sender { get; set; }
        public string SenderUserName { get; set; }
        public int ReceiverId { get; set; }
        [ForeignKey("ReceiverId")]
        public MavUser Receiver { get; set; }
        public string ReceiverUserName { get; set; }
        public string MessageText { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime SentDate { get; set; } = DateTime.UtcNow;
        public bool IsDeletedBySender { get; set; }
        public bool IsDeletedByReceiver { get; set; }
        public byte[] File { get; set; }
    }
}

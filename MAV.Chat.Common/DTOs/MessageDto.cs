using MAV.Chat.Common.DTOs;
using System;
using System.Text.Json.Serialization;

namespace MAV.Chat.Common.DTOs
{
    public class MessageDto
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public virtual MemberDto Sender { get; set; }
        public int ReceiverId { get; set; }
        public virtual MemberDto Receiver { get; set; }
        public string MessageText { get; set; }
        public byte[] UploadFile { get; set; }
        public DateTime? DateRead { get; set; } 
        public DateTime SentDate { get; set; }
        public bool IsDeletedBySender { get; set; }
        public bool IsDeletedByReceiver { get; set; }
    }
}
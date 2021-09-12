using Microsoft.AspNetCore.Http;

namespace MAV.Chat.Common.DTOs
{
    public class CreateMessageDto
    {
        public string ReceiverUserName { get; set; }
        public string MessageText { get; set; }
        public IFormFile File { get; set; }
    }
}
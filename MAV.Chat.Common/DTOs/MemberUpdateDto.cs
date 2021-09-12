using Microsoft.AspNetCore.Http;

namespace MAV.Chat.Common.DTOs
{
    public class MemberUpdateDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public IFormFile ProfilePhoto { get; set; }
        public string PhoneNumber { get; set; }
        public string About { get; set; }
    }
}
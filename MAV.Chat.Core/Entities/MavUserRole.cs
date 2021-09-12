using Microsoft.AspNetCore.Identity;

namespace MAV.Chat.Core.Entities
{
    public class MavUserRole : IdentityUserRole<int>
    {
        public MavUser User { get; set; }
        public MavRole Role { get; set; }
    }
}

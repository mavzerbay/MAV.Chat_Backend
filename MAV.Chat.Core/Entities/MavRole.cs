using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace MAV.Chat.Core.Entities
{
    public class MavRole : IdentityRole<int>
    {
        public ICollection<MavUserRole> UserRoles { get; set; }

    }
}

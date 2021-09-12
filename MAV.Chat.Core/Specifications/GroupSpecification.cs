using MAV.Chat.Common.Helpers;
using MAV.Chat.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAV.Chat.Core.Specifications
{
    public class GroupSpecification : BaseSpecification<Group>
    {
        public GroupSpecification(string groupNameOrConnectionId, bool byConnectionId = false) : base(x => byConnectionId ? x.Connections.Any(c => c.ConnectionId == groupNameOrConnectionId) : x.Name == groupNameOrConnectionId)
        {
            AddInclude(x => x.Connections);
        }
    }
}

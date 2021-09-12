using MAV.Chat.Common.Helpers;
using MAV.Chat.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAV.Chat.Core.Specifications
{
    public class MessageSpecificationForCount : BaseSpecification<Message>
    {
        public MessageSpecificationForCount(MessageSpecParams messageSpecParams)
               : base(x =>
                   (string.IsNullOrEmpty(messageSpecParams.Search) || x.MessageText.ToLower().Contains(messageSpecParams.Search.ToLower())))
        {
        }
    }
}

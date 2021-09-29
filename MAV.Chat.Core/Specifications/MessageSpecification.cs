using MAV.Chat.Common.Helpers;
using MAV.Chat.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAV.Chat.Core.Specifications
{
    public class MessageSpecification : BaseSpecification<Message>
    {
        public MessageSpecification(MessageSpecParams messageSpecParams)
               : base(x =>
                   (string.IsNullOrEmpty(messageSpecParams.Search) || x.MessageText.ToLower().Contains(messageSpecParams.Search.ToLower())))
        {


            ApplyPaging(messageSpecParams.PageSize * (messageSpecParams.PageIndex - 1), messageSpecParams.PageSize);

            AddInclude(x => x.Sender);
            AddInclude(x => x.Receiver);
            if (messageSpecParams.GetMessageThread)
            {
                AddCriteria(m => m.Receiver.UserName == messageSpecParams.UserName && m.Sender.UserName == messageSpecParams.ReceiverUserName || m.Receiver.UserName == messageSpecParams.ReceiverUserName && m.Sender.UserName == messageSpecParams.UserName);
                AddOrderBy(x => x.SentDate);
            }
            else
            {
                AddOrderByDescending(x => x.SentDate);
            }
            if (!string.IsNullOrEmpty(messageSpecParams.Container))
            {
                switch (messageSpecParams.Container)
                {
                    case "Inbox":
                        AddCriteria(x => x.ReceiverUserName == messageSpecParams.UserName);
                        break;
                    case "Outbox":
                        AddCriteria(x => x.SenderUserName == messageSpecParams.UserName);
                        break;
                    default:
                        AddCriteria(x => x.ReceiverUserName == messageSpecParams.UserName && x.ReadDate == null);
                        break;
                }
            }
        }

        public MessageSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.Sender);
            AddInclude(x => x.Receiver);
        }
    }
}

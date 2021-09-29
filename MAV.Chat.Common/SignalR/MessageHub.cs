using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MAV.Chat.Common.DTOs;
using MAV.Chat.Common.Extensions;
using MAV.Chat.Common.Helpers;
using MAV.Chat.Core.Entities;
using MAV.Chat.Core.Interfaces;
using MAV.Chat.Core.Specifications;
using Microsoft.AspNetCore.SignalR;

namespace MAV.Chat.Common.SignalR
{
    public class MessageHub : Hub
    {
        private readonly IMapper _mapper;
        private readonly IHubContext<PresenceHub> _presenceHub;
        private readonly PresenceTracker _tracker;
        private readonly IUnitOfWork _unitOfWork;

        public MessageHub(IMapper mapper, IHubContext<PresenceHub> presenceHub, PresenceTracker tracker, IUnitOfWork unitOfWork)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _presenceHub = presenceHub ?? throw new ArgumentNullException(nameof(presenceHub));
            _tracker = tracker ?? throw new ArgumentNullException(nameof(tracker));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var otherUser = httpContext.Request.Query["user"].ToString();
            var groupName = GetGroupName(Context.User.GetUserName(), otherUser);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            var group = await AddToGroup(groupName);
            await Clients.Group(groupName).SendAsync("UpdatedGroup", group);

            var currentUserName = Context.User.GetUserName();

            var spec = new MessageSpecification(new MessageSpecParams() { GetMessageThread = true, UserName = currentUserName, ReceiverUserName = otherUser });
            var messages = await _unitOfWork.Repository<Message>().ListAsync(spec);

            var unreadMessages = messages.Where(m => m.ReadDate == null && m.ReceiverUserName == currentUserName).ToList();

            if (unreadMessages.Any())
            {
                for (int i = 0; i < unreadMessages.Count; i++)
                {
                    unreadMessages[i].ReadDate = DateTime.UtcNow;
                }
            }

            var data = _mapper.Map<IReadOnlyList<Message>, IReadOnlyList<MessageDto>>(messages);

            if (_unitOfWork.HasChanges()) await _unitOfWork.SaveChangesAsync();

            await Clients.Caller.SendAsync("ReceiveMessageThread", data);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var group = await RemoveFromMessageGroup();
            await Clients.Group(group.Name).SendAsync("UpdatedGroup", group);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(CreateMessageDto createMessageDto)
        {
            var username = Context.User.GetUserName();

            if ((string.IsNullOrEmpty(createMessageDto.MessageText) || string.IsNullOrWhiteSpace(createMessageDto.MessageText)) && (createMessageDto.File == null || (createMessageDto.File != null && createMessageDto.File.Length <= 0)))
                throw new HubException("Mesaj veya dosya göndermek zorundasınız !");

            if (username == createMessageDto.ReceiverUserName.ToLower())
                throw new HubException("You cannot send messages to yourself");

            var spec = new UserSpecification(username);

            var sender = await _unitOfWork.Repository<MavUser>().GetEntityWithSpec(spec);

            spec = new UserSpecification(createMessageDto.ReceiverUserName);

            var receiver = await _unitOfWork.Repository<MavUser>().GetEntityWithSpec(spec);

            if (receiver == null) throw new HubException("Not found user");

            var message = new Message
            {
                Sender = sender,
                Receiver = receiver,
                SenderUserName = sender.UserName,
                ReceiverUserName = receiver.UserName,
                MessageText = createMessageDto.MessageText,
            };

            if (createMessageDto.File != null && createMessageDto.File.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    createMessageDto.File.CopyTo(ms);
                    byte[] fileBytes = ms.ToArray();
                    if (fileBytes != null && fileBytes.Length > 0)
                    {
                        message.File = fileBytes;
                    }
                }
            }
            var groupName = GetGroupName(sender.UserName, receiver.UserName);

            var groupSpec = new GroupSpecification(groupName);
            var group = await _unitOfWork.Repository<Group>().GetEntityWithSpec(groupSpec);

            if (group.Connections.Any(x => x.UserName == receiver.UserName))
            {
                message.ReadDate = DateTime.UtcNow;
            }
            else
            {
                var connections = await _tracker.GetConnectionsForUser(receiver.UserName);
                if (connections != null)
                {
                    await _presenceHub.Clients.Clients(connections).SendAsync("NewMessageReceived", new { nameSurname = sender.NameSurname , message = message.MessageText, messageFile = message.File });
                }
            }

            _unitOfWork.Repository<Message>().Add(message);

            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                await Clients.Group(groupName).SendAsync("NewMessage", _mapper.Map<MessageDto>(message));
            }
        }

        private async Task<Group> AddToGroup(string groupName)
        {
            var groupSpec = new GroupSpecification(groupName);
            var group = await _unitOfWork.Repository<Group>().GetEntityWithSpec(groupSpec);
            var connection = new Connection(Context.ConnectionId, Context.User.GetUserName());

            if (group == null)
            {
                group = new Group(groupName);
                _unitOfWork.Repository<Group>().Add(group);
            }

            group.Connections.Add(connection);

            if (await _unitOfWork.SaveChangesAsync() > 0) return group;

            throw new HubException("Failed to join group");
        }

        private async Task<Group> RemoveFromMessageGroup()
        {
            var groupSpec = new GroupSpecification(Context.ConnectionId, true);
            var group = await _unitOfWork.Repository<Group>().GetEntityWithSpec(groupSpec);
            var connection = group.Connections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);

            _unitOfWork.Repository<Connection>().Delete(connection);

            if (await _unitOfWork.SaveChangesAsync() > 0) return group;

            throw new HubException("Failed to remove from group");

        }

        private string GetGroupName(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }
    }
}
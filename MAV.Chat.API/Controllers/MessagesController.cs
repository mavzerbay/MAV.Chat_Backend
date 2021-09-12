using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using MAV.Chat.Common.DTOs;
using MAV.Chat.Common.Extensions;
using MAV.Chat.Common.Helpers;
using MAV.Chat.Core.Entities;
using MAV.Chat.Core.Interfaces;
using MAV.Chat.Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MAV.Chat.API.Controllers
{
    [Authorize]
    public class MessagesController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public MessagesController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            var username = User.GetUserName();

            if ((string.IsNullOrEmpty(createMessageDto.MessageText) || string.IsNullOrWhiteSpace(createMessageDto.MessageText)) && (createMessageDto.File == null || (createMessageDto.File != null && createMessageDto.File.Length <= 0)))
                return BadRequest("Mesaj veya dosya göndermek zorundasınız !");


            if (username == createMessageDto.ReceiverUserName.ToLower())
                return BadRequest("Kendinize mesaj atamazsınız !");

            var spec = new UserSpecification(username);

            var sender = await _unitOfWork.Repository<MavUser>().GetEntityWithSpec(spec);

            spec = new UserSpecification(createMessageDto.ReceiverUserName);
            var receiver = await _unitOfWork.Repository<MavUser>().GetEntityWithSpec(spec);

            if (receiver == null) return NotFound();

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

            _unitOfWork.Repository<Message>().Add(message);

            if (await _unitOfWork.SaveChangesAsync() > 0) return Ok(_mapper.Map<MessageDto>(message));

            return BadRequest("Failed to send message");

        }

        [HttpGet]
        public async Task<ActionResult<Pagination<IEnumerable<MessageDto>>>> GetMessagesForUser([FromQuery] MessageSpecParams messageSpecParams)
        {
            messageSpecParams.UserName = User.GetUserName();

            var spec = new MessageSpecification(messageSpecParams);
            var specCount = new MessageSpecificationForCount(messageSpecParams);

            var totalItems = await _unitOfWork.Repository<Message>().CountAsync(specCount);

            var messages = await _unitOfWork.Repository<Message>().ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<Message>, IReadOnlyList<MessageDto>>(messages);

            return Ok(new Pagination<MessageDto>(messageSpecParams.PageIndex, messageSpecParams.PageSize, totalItems, data));
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult> DeleteMessage(int Id)
        {
            var username = User.GetUserName();

            var spec = new MessageSpecification(Id);
            var message = await _unitOfWork.Repository<Message>().GetEntityWithSpec(spec);

            if (message.Sender.UserName != username && message.Receiver.UserName != username)
                return Unauthorized();

            if (message.Sender.UserName == username)
                message.IsDeletedBySender = true;

            if (message.Receiver.UserName == username)
                message.IsDeletedByReceiver = true;


            if (await _unitOfWork.SaveChangesAsync() > 0) return Ok();

            return BadRequest("Problem deleting the message");
        }
    }
}
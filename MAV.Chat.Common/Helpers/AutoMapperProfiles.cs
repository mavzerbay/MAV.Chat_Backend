using AutoMapper;
using MAV.Chat.Common.DTOs;
using MAV.Chat.Core.Entities;

namespace MAV.Chat.Common.Helpers
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<MavUser, MemberDto>();
            CreateMap<MemberUpdateDto, MavUser>()
                .ForMember(dest=>dest.ProfilePhoto,opt=>opt.Ignore());
            CreateMap<RegisterDto, MavUser>();
            CreateMap<Message, MessageDto>();
        }
    }
}

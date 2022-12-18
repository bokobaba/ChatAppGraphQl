using AutoMapper;
using ChatAppGraphQl.DTOs.MessageDtos;
using ChatAppGraphQl.DTOs.UserDtos;
using ChatAppGraphQl.Model;

namespace ChatAppGraphQl {
    public class AutoMapperProfile: Profile {
        public AutoMapperProfile() {
            CreateMap<AddUserDto, User>();
            CreateMap<User, GetUserDto>();

            CreateMap<AddMessageDto, Message>();
            CreateMap<Message, GetMessageDto>();
        }
    }
}

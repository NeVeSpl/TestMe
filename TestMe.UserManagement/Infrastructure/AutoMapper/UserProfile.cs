using AutoMapper;
using TestMe.UserManagement.App.Users.Output;
using TestMe.UserManagement.Domain;

namespace TestMe.UserManagement.Infrastructure.AutoMapper
{
    internal sealed class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>()                
                .ForMember(x => x.EmailAddress, opt => opt.MapFrom(o => o.EmailAddress.Value));
        }
    }
}

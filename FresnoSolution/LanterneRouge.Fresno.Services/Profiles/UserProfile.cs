using AutoMapper;
using LanterneRouge.Fresno.Core.Entity;
using LanterneRouge.Fresno.Core.Interface;
using LanterneRouge.Fresno.Services.Models;

namespace LanterneRouge.Fresno.Services.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserModel>();
            CreateMap<IUserEntity, User>();
        }
    }
}

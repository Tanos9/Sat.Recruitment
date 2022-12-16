using AutoMapper;
using Sat.Recruitment.Api.Models.User;

namespace Sat.Recruitment.Api.Utils
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserCreateRequest, User>();
        }
    }
}
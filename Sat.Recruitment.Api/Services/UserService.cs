using AutoMapper;
using Sat.Recruitment.Api.DataAccess;
using Sat.Recruitment.Api.Models;
using Sat.Recruitment.Api.Models.Enums;
using Sat.Recruitment.Api.Models.User;
using Sat.Recruitment.Api.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<User> _genericRepository;


        public UserService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<Result> CreateUser(UserCreateRequest userCreateRequest)
        {
            var user = _mapper.Map<User>(userCreateRequest);


            return await Task<Result>.FromResult(new Result() { Errors = "nice" });
        }

        private void AddMoneyGiftByUserType(User user)
        {
            decimal percentage = GetPercentageByUserType(user);

            if (percentage > 0)
               user.Money += CalculateMoneyGift(user.Money, percentage);
        }

        private decimal GetPercentageByUserType(User user)
        {
            if (EnumEquals(user.UserType, UserType.Normal))
            {
                if (user.Money > 100)
                {
                    return (decimal)0.12;
                }

                if (user.Money > 10 && user.Money < 100)
                {
                    return (decimal)0.8;
                }
            }

            if (EnumEquals(user.UserType, UserType.SuperUser))
            {
                if (user.Money > 100)
                {
                    return (decimal)0.20;
                }
            }

            if (EnumEquals(user.UserType, UserType.Normal))
            {
                if (user.Money > 100)
                {
                    return 2;
                }
            }

            return 0;
        }

        private decimal CalculateMoneyGift(decimal money, decimal percentage)
        {
            return money * percentage;
        }

        public bool EnumEquals(string enumString, UserType value)
        {
            if (Enum.TryParse<UserType>(enumString, out var v))
            {
                return value == v;
            }

            return false;
        }
    }
}

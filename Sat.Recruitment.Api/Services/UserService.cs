using AutoMapper;
using Microsoft.Extensions.Logging;
using Sat.Recruitment.Api.DataAccess.Interfaces;
using Sat.Recruitment.Api.Models;
using Sat.Recruitment.Api.Models.Enums;
using Sat.Recruitment.Api.Models.User;
using Sat.Recruitment.Api.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserJsonRepository _userJsonRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IMapper mapper, IUserJsonRepository repository, ILogger<UserService> logger)
        {
            _mapper = mapper;
            _userJsonRepository = repository;
            _logger = logger;
        }

        public async Task<Result> CreateUser(UserCreateRequest userCreateRequest)
        {
            var user = _mapper.Map<User>(userCreateRequest);

            var userExists = await _userJsonRepository.UserExists(user);

            if (userExists)
            {
                return new Result()
                {
                    IsSuccess = false,
                    Errors = "The user already exists."
                };
            }

            AddMoneyGiftByUserType(user);

            await _userJsonRepository.Insert(user);

            _logger.LogInformation($"User Created: {user.Name}" + " - " + $"{user.Address}");

            return new Result()
            {
                IsSuccess = true,
                Errors = "The user has been created"
            };
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _userJsonRepository.GetAll();
        }


        public void AddMoneyGiftByUserType(User user)
        {
            decimal percentage = GetPercentageByUserType(user);

            if (percentage > 0)
            {
                user.Money += CalculateMoneyGift(user.Money, percentage);
            }
        }

        private decimal GetPercentageByUserType(User user)
        {
            if (Equals(user.UserType, UserType.Normal))
            {
                if (user.Money > 100)
                {
                    return (decimal)0.12;
                }

                if (user.Money > 10 && user.Money < 100)
                {
                    return (decimal)0.8;
                }

                return 0;
            }

            if (Equals(user.UserType, UserType.SuperUser))
            {
                if (user.Money > 100)
                {
                    return (decimal)0.20;
                }

                return 0;
            }

            if (Equals(user.UserType, UserType.Normal))
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

        private bool Equals(string stringValue, UserType enumValue)
        {
            var result = stringValue.ToLower().Equals(enumValue.ToString().ToLower());
            return result;
        }
    }
}

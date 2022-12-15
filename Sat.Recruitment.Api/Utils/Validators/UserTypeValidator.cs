using Sat.Recruitment.Api.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Sat.Recruitment.Api.Utils.Validators
{
    public sealed class UserTypeValidator : ValidationAttribute
    {
        private List<UserType> _enumUserTypes = Enum.GetValues(typeof(UserType)).Cast<UserType>().ToList();
        private List<string> _userTypes;

        public UserTypeValidator()
        {
            _userTypes = _enumUserTypes.ConvertAll(u => u.ToString().ToLower());
        }

        public override bool IsValid(object value)
        {
            var isValid = false;
            var strValue = value as string;

            if (!string.IsNullOrEmpty(strValue))
            {
                return _userTypes.Contains(strValue.ToLower());
            }

            return isValid;
        }
    }
}

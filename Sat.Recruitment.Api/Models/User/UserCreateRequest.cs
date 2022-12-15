using Sat.Recruitment.Api.Utils.Validators;
using System.ComponentModel.DataAnnotations;

namespace Sat.Recruitment.Api.Models.User
{
    public sealed class UserCreateRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        [UserTypeValidator]
        public string UserType { get; set; }

        [Required]
        public decimal Money { get; set; }
    }
}

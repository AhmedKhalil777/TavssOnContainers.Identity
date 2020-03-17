using Identity.Api.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Api.Contracts.V1.Requests
{
    public class RegisterDoctorTAViewModel
    {
        [Required]
        [Display(Name ="FullName")]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Compare("Password",ErrorMessage ="The Password doesn't match the confirm")]
        public string ConfirmPassword { get; set; }
        [Required]
        public Departments Department { get; set; }
    }
}

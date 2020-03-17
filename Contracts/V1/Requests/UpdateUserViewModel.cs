using Identity.Api.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Api.Contracts.V1.Requests
{
    public interface IUpdateUserViewModel
    {

    }
    public class UpdateUserViewModel : IUpdateUserViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public Departments Department { get; set; }
        public string StudyYear { get; set; }
        [Required]
        public string Id { get; set; }


    }

    
}

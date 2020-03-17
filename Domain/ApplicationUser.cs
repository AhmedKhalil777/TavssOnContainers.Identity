using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Api.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string PicPath { get; set; }
        public Departments Department { get; set; }
        
        public string StudyYear { get; set; }

    }
}

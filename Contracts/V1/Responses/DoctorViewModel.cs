using Identity.Api.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Api.Contracts.V1.Responses
{
    public class DoctorViewModel : IViewModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PicPath { get; set; }
        public Departments Department { get; set; }
      
    }
}

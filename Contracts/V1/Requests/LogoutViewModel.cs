using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Api.Contracts.V1.Requests
{
    public class LogoutViewModel
    {
        public string LogoutId { get; set; }
    }
}

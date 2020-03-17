using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Api.Domain
{
    public class TA 
    {
        [Required]
        [Key]
        public int Id { get; set; }
        public string PicPath { get; set; }
        [Required]
        public Departments Department { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
